using Almostengr.LightShowExtender.Worker;
using Almostengr.LightShowExtender.DomainService.Common;
using Almostengr.LightShowExtender.Infrastructure.Logging;
using Almostengr.LightShowExtender.DomainService.Jukebox;
using Almostengr.LightShowExtender.DomainService.Monitoring;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.Sources.Clear();
    })
    .ConfigureServices(services =>
    {
        services.AddSingleton<AppSettings>(); // todo review for correctness

        services.AddHostedService<MonitoringWorker>();
        services.AddHostedService<JukeboxWorker>();

        services.AddSingleton<IMonitoringService, MonitoringService>();
        services.AddSingleton<IJukeboxService, JukeboxService>();

        services.AddSingleton(typeof(ILoggingService<>), typeof(LoggingService<>));
    })
    .UseSystemd()
    .Build();

await host.RunAsync();
