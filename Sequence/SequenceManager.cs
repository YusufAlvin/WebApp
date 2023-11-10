using LoggingLibrary;
using Data.Dto;
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
    private int[] _currentPosition = new int[] {0,0};

    public event EventHandler<SequenceProgress> OnSequenceProgressChanged;

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

        _messageHub.Subscribe<RobotEventArgs>(RobotChangedHandler);
    }

    private void RobotChangedHandler(RobotEventArgs arg)
    {
        _status = arg.Status;
        _currentPosition = arg.CurrentPosition;

        OnSequenceProgressChangedHandler();
    }

    private void OnSequenceProgressChangedHandler()
    {
        var message = new SequenceProgress()
        {
            Status = _status.ToString(),
            CurrentPosition = _currentPosition,
        };
        OnSequenceProgressChanged?.Invoke(this, message);
    }

    public bool Start(SequenceDto sequenceDto)
    {
        if (Status != Status.Idle && Status != Status.Stopped)
        {
            _logger.Info("System is busy");
            return false;
        }
        Task.Run(async () =>
        {
            Status = Status.Running;
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

    public void Stop()
    {
        _robotManager.Stop();
    }

    public void Pause()
    {
        _robotManager.Pause();
        _pauseEventSlim.Reset();
    }

    public void Resume()
    {
        _robotManager.Resume();  
        _pauseEventSlim.Set();
    }
}
