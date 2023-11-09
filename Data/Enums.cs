namespace Data;

public static class HubEvents
{
    public static string OnSequenceProgress => "OnSequenceProgress";
}

public enum Status
{
    Idle,
    Running,
    Paused,
    Stopped,
    Done,
}

public class SequenceProgress
{
    public string Status { get; set; }
    public int[] CurrentPosition { get; set; }

    public SequenceProgress()
    {
        Status = "Idle";
        CurrentPosition = new int[] {1,1};
    }
}
