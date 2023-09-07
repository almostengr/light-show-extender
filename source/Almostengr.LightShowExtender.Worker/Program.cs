using Almostengr.LightShowExtender.Worker;
using Almostengr.LightShowExtender.DomainService.Common;
using Almostengr.LightShowExtender.Infrastructure.Logging;
using Almostengr.LightShowExtender.DomainService.Jukebox;
using Almostengr.LightShowExtender.DomainService.Monitoring;
using Almostengr.LightShowExtender.DomainService.Common.Constants;
using Almostengr.LightShowExtender.DomainService.TheAlmostEngineer;
using Almostengr.LightShowExtender.DomainService.FalconPiPlayer;
using Almostengr.LightShowExtender.Infrastructure.TheAlmostEngineer;
using Almostengr.LightShowExtender.Infrastructure.FalconPiPlayer;
using Almostengr.LightShowExtender.DomainService.NwsWeather;
using Almostengr.LightShowExtender.Infrastructure.NwsWeather;

Console.WriteLine(typeof(Program).Assembly.ToString());

string environment = AppEnvironment.Prod;

#if !RELEASE
environment = AppEnvironment.Devl;
#endif

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile(
        (environment == AppEnvironment.Prod) ? AppConstants.AppSettingsProdFile : AppConstants.AppSettingsDevlFile,
        false,
        false)
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

        services.AddSingleton<IFppHttpClient, FppHttpClient>();
        services.AddSingleton<IEngineerHttpClient, EngineerHttpClient>();
        services.AddSingleton<INwsHttpClient, NwsHttpClient>();

        services.AddSingleton<IMonitoringService, MonitoringService>();
        services.AddSingleton<IJukeboxService, JukeboxService>();

        services.AddSingleton(typeof(ILoggingService<>), typeof(LoggingService<>));

        services.AddHostedService<JukeboxWorker>();
        services.AddHostedService<MonitoringWorker>();
        // services.AddHostedService<WeatherWorker>();
    })
    .UseSystemd()
    .Build();

await host.RunAsync();
