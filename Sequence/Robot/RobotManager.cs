using Easy.MessageHub;
using LoggingLibrary;
using Robot.EventArguments;
using Data;

namespace Robot;

public class RobotManager : IRobotManager
{
    private Motor _xm;
    private Motor _ym;
    private readonly LoggerService<RobotManager> _logger;
    private readonly IMessageHub _messageHub;
    private RobotEventArgs _eventArgs = new RobotEventArgs();

    public RobotManager(LoggerService<RobotManager> logger, IMessageHub messageHub, Motor motorX, Motor motorY)
    {
        _xm = motorX;
        _ym = motorY;
        _logger = logger;
        _messageHub = messageHub;
        _eventArgs.Status = Status.Idle;
        _eventArgs.CurrentPosition = new int[] { 0, 0 };

        _xm.SetName("x");
        _ym.SetName("y");
        _xm.MotorChanged += MotorChangedHandler;
        _ym.MotorChanged += MotorChangedHandler;
    }

    protected virtual void MotorChangedHandler(object sender, MotorEventArgs args)
    {
        _eventArgs.Status = args.Status;

        switch(args.Name)
        {
            case "x":
                _eventArgs.CurrentPosition[0] = args.Position;
                break;
            case "y":
                _eventArgs.CurrentPosition[1] = args.Position;
                break;
        }

        PublishRobotEvent();
    }

    private void PublishRobotEvent()
    {
        _messageHub.Publish(_eventArgs);
    }

    public async Task MoveTo(int x, int y)
    {

        Task motorX = Task.Run(() => _xm.MoveTo(x));
        Task motorY = Task.Run(() => _ym.MoveTo(y));
        await Task.WhenAll(motorX, motorY);
        _logger.Info($"Current Position: ({_xm.GetPosition()}, {_ym.GetPosition()})");
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
