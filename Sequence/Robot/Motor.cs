using LoggingLibrary;
using Robot.Api;
using Robot.Enums;

namespace Robot;

public class Motor : IMotor
{
    private MotorAPI _motorAPI;
    private int _position, _speed;
    private Status _status = Status.Idle;
    private AutoResetEvent _waitPause;
    private string? _name;
    private readonly LoggerService<Motor> _logger;
    public event EventHandler<bool> StopChanged;

    public Motor(LoggerService<Motor> logger)
    {
        _position = 1;
        _speed = 4000;
        _motorAPI = new MotorAPI();
        _waitPause = new AutoResetEvent(true);
        _logger = logger;
    }

    protected void StopChangeHandler(bool value)
    {
        StopChanged?.Invoke(this, value);
    }

    public int GetPosition()
    {
        return _position;
    }

    public void MoveTo(int destination)
    {
        _status = Status.Running;

        if (_position < destination)
        {
            while (_position != destination)
            {

                if (_status == Status.Stopped) break;

                if (_status == Status.Paused)
                {
                    _waitPause.WaitOne();
                }

                _position++;
                _motorAPI.MoveTo(_position);
                _logger.Info($"{_name}: {_position}");
                Thread.Sleep(_speed);

                if (_status == Status.Stopped) break;

                if (_status == Status.Paused)
                {
                    _waitPause.WaitOne();
                }
            }
            return;
        }

        if (_position > destination)
        {
            while (_position != destination)
            {
                if (_status == Status.Stopped) break;

                if (_status == Status.Paused)
                {
                    _waitPause.WaitOne();
                }

                _position--;
                _logger.Info($"{_name}: {_position}");
                _motorAPI.MoveTo(_position);
                Thread.Sleep(_speed);

                if (_status == Status.Stopped) break;

                if (_status == Status.Paused)
                {
                    _waitPause.WaitOne();
                }
            }
            return;
        }

        return;
    }

    public void Stop()
    {
        _status = Status.Stopped;
    }

    public void Pause()
    {
        _status = Status.Paused;
        _waitPause.Reset();
    }

    public void Resume()
    {
        _status = Status.Running;
        _waitPause.Set();
    }

    public void SetName(string name)
    {
        _name = name;
    }
}