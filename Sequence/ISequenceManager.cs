using Robot.EventArguments;
using Robot.Sequence.Data;

namespace Robot;

public interface ISequenceManager
{
    //public event EventHandler<PositionChangedEventArgs> PositionChanged;
    //public event EventHandler<string> StatusChanged;
    //void AddSequence(string filePath);
    bool Start(SequenceData sequenceData);
    void Stop();
    void Pause();
    void Resume();
}
