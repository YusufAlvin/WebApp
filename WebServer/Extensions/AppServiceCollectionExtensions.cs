using LoggingLibrary;
using Robot;
using Robot.Api;

public static class AppServiceCollectionExtensions
{
    public static void AddAppServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton(typeof(LoggerService<>));
        serviceCollection.AddSingleton<MotorAPI>();
        serviceCollection.AddTransient<Motor>();
        serviceCollection.AddSingleton<RobotManager>();
        serviceCollection.AddSingleton<SequenceManager>();
    }
}
