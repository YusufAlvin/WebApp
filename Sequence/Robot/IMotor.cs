namespace Robot;

public interface IMotor
{
    void MoveTo(int n);
    int GetPosition();
    void Stop();
    void Pause();
    void Resume();
    void SetName(string name);
}