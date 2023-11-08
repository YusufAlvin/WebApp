using LoggingLibrary;
using Robot.Enums;
using Robot.Sequence.Data;

namespace Robot;

public class SequenceManager : ISequenceManager
{
    private RobotManager _robotManager;
    private object statusLock = new object();
    private Status _status;
    private ManualResetEventSlim _pauseEventSlim;
    private readonly LoggerService<SequenceManager> _logger;
    public Status Status
    {
        get { lock (statusLock) { return _status; } }
        private set { lock (statusLock) { _status = value; } }
    }

    public SequenceManager(RobotManager robotManager, LoggerService<SequenceManager> logger)
    {
        _status = Status.Idle;
        _robotManager = robotManager;
        _pauseEventSlim = new ManualResetEventSlim(false);
        _logger = logger;
    }

    public bool Start(SequenceData sequenceData)
    {
        if (Status != Status.Idle)
        {
            _logger.Info("System is busy");
            return false;
        }
        Task.Run(async () =>
        {
            Status = Status.Running;
            foreach (var sequence in sequenceData.Sequence)
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
        Status = Status.Stopped;
        _robotManager.Stop();
    }

    public void Pause()
    {
        _robotManager.Pause();
        Status = Status.Paused;
        _pauseEventSlim.Reset();
    }

    public void Resume()
    {
        _robotManager.Resume();
        Status = Status.Running;        
        _pauseEventSlim.Set();
    }
}
