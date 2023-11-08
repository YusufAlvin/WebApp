using log4net;
using log4net.Config;

namespace LoggingLibrary;

public class LoggerService<T> : ILoggerService
{
    private readonly ILog _log;
    public LoggerService()
    {
        XmlConfigurator.Configure(new FileInfo(@"Config\log4net.config"));
        _log = LogManager.GetLogger(typeof(T));
    }

    public void Error(string message)
    {         
        _log.Error(message);
    }

    public void Info(string message)
    {
        _log.Info(message);
    }

    public void Warning(string message)
    {
        _log.Warn(message);
    }
}