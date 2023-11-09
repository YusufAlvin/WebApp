using Robot.EventArguments;
using Data.Dto;

namespace Robot;

public interface ISequenceManager
{
    //public event EventHandler<PositionChangedEventArgs> PositionChanged;
    //public event EventHandler<string> StatusChanged;
    //void AddSequence(string filePath);
    bool Start(SequenceDto sequenceDto);
    void Stop();
    void Pause();
    void Resume();
}
