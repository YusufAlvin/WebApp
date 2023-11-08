namespace Robot;

public interface IRobotManager
{
    Task MoveTo(int x, int y);
    void Stop();
    void Pause();
    void Resume();
}
