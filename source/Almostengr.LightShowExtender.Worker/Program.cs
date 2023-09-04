using Almostengr.LightShowExtender.Worker;
using Almostengr.LightShowExtender.DomainService.Common;
using Almostengr.LightShowExtender.Infrastructure.Logging;
using Almostengr.LightShowExtender.DomainService.Jukebox;
using Almostengr.LightShowExtender.DomainService.Monitoring;
using Almostengr.LightShowExtender.DomainService.Common.Constants;

Console.WriteLine(typeof(Program).Assembly.ToString());

string environment = string.Empty;

#if RELEASE
    environment = AppEnvironment.Prod;
#else
environment = AppEnvironment.Devl;
#endif

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile(
        (environment == AppEnvironment.Prod) ? AppConstants.AppSettingsProdFile : AppConstants.AppSettingsDevlFile,
        false,
        true)
    .Build();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.Sources.Clear();
    })
    .ConfigureServices(services =>
    {
        AppSettings appSettings = configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
        services.AddSingleton(appSettings);

        services.AddSingleton<IMonitoringService, MonitoringService>();
        services.AddSingleton<IJukeboxService, JukeboxService>();

        services.AddSingleton(typeof(ILoggingService<>), typeof(LoggingService<>));

        services.AddHostedService<JukeboxWorker>();
        services.AddHostedService<MonitoringWorker>();
        services.AddHostedService<WeatherWorker>();
    })
    .UseSystemd()
    .Build();

await host.RunAsync();
