using LoggingLibrary;
using Robot.Enums;
using Robot.Sequence.Data;

namespace Robot;

public class SequenceManager : ISequenceManager
{
    private RobotManager _robotManager;
    private Status _status;
    private ManualResetEventSlim _pauseEventSlim;
    private object _lock = new object();
    private readonly LoggerService<SequenceManager> _logger;

    public SequenceManager(RobotManager robotManager, LoggerService<SequenceManager> logger)
    {
        _status = Status.Idle;
        _robotManager = robotManager;
        _pauseEventSlim = new ManualResetEventSlim(false);
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        _logger = logger;
    }

    public bool Start(SequenceData sequenceData)
    {
        lock (_lock)
        {
            if (_status == Status.Idle)
            {
                _status = Status.Running;
                Task.Run(async () =>
                {
                    foreach (var sequence in sequenceData.Sequence)
                    {
                        if (_status == Status.Stopped) break;

                        if (_status == Status.Paused) _pauseEventSlim.Wait();

                        _logger.Info($"run sequence {sequence.Id}");
                        await _robotManager.MoveTo(sequence.Position[0], sequence.Position[1]);

                        if (_status == Status.Stopped) break;

                        if (_status == Status.Paused) _pauseEventSlim.Wait();

                        _logger.Info($"sequence {sequence.Id} is done");
                    }
                    _status = Status.Done;
                });
                return true;
            }

            _logger.Info("System is busy");
            return false;
        }
    }

    public void Stop()
    {
        _status = Status.Stopped;
        _robotManager.Stop();
    }

    public void Pause()
    {
        _robotManager.Pause();
        _status = Status.Paused;
        _pauseEventSlim.Reset();
    }

    public void Resume()
    {
        _robotManager.Resume();
        _status = Status.Running;
        _pauseEventSlim.Set();
    }
}
