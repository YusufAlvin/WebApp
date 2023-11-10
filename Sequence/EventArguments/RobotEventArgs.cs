using Data;

namespace Robot.EventArguments;

public class RobotEventArgs
{
    public Status Status { get; set; }
    public int[] CurrentPosition { get; set; }
}
