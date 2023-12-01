using Easy.MessageHub;
using LoggingLibrary;
using Robot;
using Robot.Api;
using WebServer;
using WebServer.Hubs;
using WebServer.Repository;

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

        serviceCollection.AddScoped<IProductRepository, ProductRepository>();
        serviceCollection.AddScoped<AppDbContext>();
    }
}
