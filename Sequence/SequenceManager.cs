using LoggingLibrary;
using Data.Dto;
using Timer = System.Timers.Timer;
using Easy.MessageHub;
using Data;
using Robot.EventArguments;

namespace Robot;

public class SequenceManager : ISequenceManager
{
    private RobotManager _robotManager;
    private object statusLock = new object();
    private Status _status;
    private ManualResetEventSlim _pauseEventSlim;
    private readonly LoggerService<SequenceManager> _logger;
    private readonly IMessageHub _messageHub;
    private readonly int[] _currentPosition = new int[] {0,0};

    public Status Status
    {
        get { lock (statusLock) { return _status; } }
        private set { lock (statusLock) { _status = value; } }
    }

    public SequenceManager(RobotManager robotManager, LoggerService<SequenceManager> logger, IMessageHub messageHub)
    {
        _status = Status.Idle;
        _robotManager = robotManager;
        _pauseEventSlim = new ManualResetEventSlim(false);
        _logger = logger;
        _messageHub = messageHub;

        _messageHub.Subscribe<PositionChangedEventArgs>(PositionChangeHandler);
    }

    private void PositionChangeHandler(PositionChangedEventArgs arg)
    {
        _currentPosition[0] = arg.x;
        _currentPosition[1] = arg.y;
        SequenceProgressChange();
    }

    public bool Start(SequenceDto sequenceDto)
    {
        if (Status != Status.Idle)
        {
            _logger.Info("System is busy");
            return false;
        }
        Task.Run(async () =>
        {
            Status = Status.Running;
            SequenceProgressChange();
            foreach (var sequence in sequenceDto.Sequences)
            {
                if (_status == Status.Stopped)
                {
                    _logger.Info("sequence stopped");
                    break;
                }
                if (_status == Status.Paused)
                {
                    _logger.Info("sequence paused");
                    _pauseEventSlim.Wait();
                }
                _logger.Info($"run sequence {sequence.Id}");
                await _robotManager.MoveTo(sequence.Position[0], sequence.Position[1]);
                _logger.Info($"sequence {sequence.Id} is done");
            }
            Status = Status.Idle;
        });
        return true;
    }

    private void SequenceProgressChange()
    {
        var message = new SequenceProgress() 
        {
            Status = _status.ToString(),
            CurrentPosition = _currentPosition,
        };
        _messageHub.Publish(message);
    }

    public void Stop()
    {
        Status = Status.Stopped;
        _robotManager.Stop();
        SequenceProgressChange();
    }

    public void Pause()
    {
        _robotManager.Pause();
        Status = Status.Paused;
        _pauseEventSlim.Reset();
        SequenceProgressChange();
    }

    public void Resume()
    {
        _robotManager.Resume();
        Status = Status.Running;        
        _pauseEventSlim.Set();
        SequenceProgressChange();
    }
}
