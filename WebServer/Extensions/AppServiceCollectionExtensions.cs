using Easy.MessageHub;
using LoggingLibrary;
using Robot;
using Robot.Api;
using WebServer.Hubs;

public static class AppServiceCollectionExtensions
{
    public static void AddAppServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSignalR();
        serviceCollection.AddSingleton<InstrumentHub>();
        serviceCollection.AddSingleton(typeof(LoggerService<>));
        serviceCollection.AddSingleton<MotorAPI>();
        serviceCollection.AddTransient<Motor>();
        serviceCollection.AddSingleton<RobotManager>();
        serviceCollection.AddSingleton<SequenceManager>();
        serviceCollection.AddSingleton<IMessageHub, MessageHub>();
    }
}
