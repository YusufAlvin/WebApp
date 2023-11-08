﻿using LoggingLibrary;
using Robot.EventArguments;

namespace Robot;

public class RobotManager : IRobotManager
{
    private Motor _xm;
    private Motor _ym;
    private readonly LoggerService<RobotManager> _logger;
    public event EventHandler<PositionChangedEventArgs>? PositionChanged;

    public RobotManager(LoggerService<RobotManager> logger, Motor motorX, Motor motorY)
    {
        _xm = motorX;
        _ym = motorY;
        _logger = logger;
        _xm.SetName("x");
        _ym.SetName("y");
    }

    protected virtual void PositionChangedHandler()
    {
        var x = _xm.GetPosition();
        var y = _ym.GetPosition();
        var eventArgs = new PositionChangedEventArgs(x, y);
        PositionChanged?.Invoke(this, eventArgs);
    }

    public async Task MoveTo(int x, int y)
    {
        Task motorX = Task.Run(() => _xm.MoveTo(x));
        Task motorY = Task.Run(() => _ym.MoveTo(y));
        await Task.WhenAll(motorX, motorY);
        _logger.Info($"Current Position: ({_xm.GetPosition()}, {_ym.GetPosition()})");
        PositionChangedHandler();
    }

    public void Stop()
    {
        _xm.Stop();
        _ym.Stop();
    }

    public void Pause()
    {
        _xm.Pause();
        _ym.Pause();
    }

    public void Resume()
    {
        _xm.Resume();
        _ym.Resume();
    }
}