using Almostengr.LightShowExtender.Worker;
using Almostengr.LightShowExtender.DomainService.Common;
using Almostengr.LightShowExtender.Infrastructure.Logging;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.Sources.Clear();
    })
    .ConfigureServices(services =>
    {
        services.AddHostedService<FppMonitoringWorker>();
        services.AddHostedService<TheAlmostEngineerWorker>();

        services.AddSingleton(typeof(ILoggingService<>), typeof(LoggingService<>));
    })
    .UseSystemd()
    .Build();

await host.RunAsync();
