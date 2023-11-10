using LoggingLibrary;
using Robot.Api;
using Data;
using Robot.EventArguments;

namespace Robot;

public class Motor : IMotor
{
    private MotorAPI _motorAPI;
    private int _position, _speed;
    private Status _status = Status.Idle;
    private AutoResetEvent _waitPause;
    private string _name = "Default Name";
    private readonly LoggerService<Motor> _logger;
    public event EventHandler<MotorEventArgs> MotorChanged;

    public Motor(LoggerService<Motor> logger)
    {
        _position = 1;
        _speed = 5000;
        _motorAPI = new MotorAPI();
        _waitPause = new AutoResetEvent(true);
        _logger = logger;
    }

    private void MotorChangedHandler()
    {
        var data = new MotorEventArgs() { Name = _name, Position = _position, Status = _status };
        MotorChanged?.Invoke(this, data);
    }

    public int GetPosition()
    {
        return _position;
    }

    public void MoveTo(int destination)
    {
        _status = Status.Running;
        MotorChangedHandler();
        if (_position < destination)
        {
            while (_position != destination)
            {
                if (_status == Status.Stopped)
                {
                    MotorChangedHandler();
                    _logger.Info($"motor {_name} stopped");
                    break;
                }

                if (_status == Status.Paused)
                {
                    MotorChangedHandler();
                    _logger.Info($"motor {_name} paused");
                    _waitPause.WaitOne();
                    MotorChangedHandler();
                }

                _position++;
                _motorAPI.MoveTo(_position);
                _logger.Info($"{_name}: {_position}");
                Thread.Sleep(_speed);

                if (_status == Status.Stopped)
                {
                    MotorChangedHandler();
                    _logger.Info($"motor {_name} stopped");
                    break;
                }

                if (_status == Status.Paused)
                {
                    MotorChangedHandler();
                    _logger.Info($"motor {_name} paused");
                    _waitPause.WaitOne();
                    MotorChangedHandler();
                }
            }
            MotorChangedHandler();
            return;
        }

        if (_position > destination)
        {
            while (_position != destination)
            {
                if (_status == Status.Stopped)
                {
                    MotorChangedHandler();
                    _logger.Info($"motor {_name} stopped");
                    break;
                }

                if (_status == Status.Paused)
                {
                    MotorChangedHandler();
                    _logger.Info($"motor {_name} paused");
                    _waitPause.WaitOne();
                    MotorChangedHandler();

                    if (_status == Status.Stopped)
                    {
                        MotorChangedHandler();
                        _logger.Info($"motor {_name} stopped");
                        break;
                    }
                }

                _position--;
                _logger.Info($"{_name}: {_position}");
                _motorAPI.MoveTo(_position);
                Thread.Sleep(_speed);

                if (_status == Status.Stopped)
                {
                    MotorChangedHandler();
                    _logger.Info($"motor {_name} stopped");
                    break;
                }

                if (_status == Status.Paused)
                {
                    MotorChangedHandler();
                    _logger.Info($"motor {_name} paused");
                    _waitPause.WaitOne();
                    MotorChangedHandler();

                    if (_status == Status.Stopped)
                    {
                        MotorChangedHandler();
                        _logger.Info($"motor {_name} stopped");
                        break;
                    }
                }
            }
            MotorChangedHandler();
            return;
        }

        return;
    }

    public void Stop()
    {
        _status = Status.Stopped;
        _waitPause.Set();
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

    public Status GetStatus()
    {
        return _status;
    }
}