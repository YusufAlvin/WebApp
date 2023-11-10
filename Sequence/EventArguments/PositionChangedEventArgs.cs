namespace Robot.EventArguments;

public class PositionChangedEventArgs : EventArgs
{
    public int x;
    public int y;

    public PositionChangedEventArgs(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
