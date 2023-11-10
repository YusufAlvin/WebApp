using Data;

namespace Robot.EventArguments;

public class MotorEventArgs : EventArgs
{
    public string Name { get; set; }
    public int Position { get; set; }
    public Status Status { get; set; }
}
