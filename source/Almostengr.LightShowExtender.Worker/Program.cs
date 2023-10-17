using Almostengr.LightShowExtender.DomainService;
using Almostengr.LightShowExtender.Worker;
using Almostengr.LightShowExtender.DomainService.Common;
using Almostengr.Common.Logging;
using Almostengr.LightShowExtender.DomainService.Common.Constants;
using Almostengr.LightShowExtender.DomainService.FalconPiPlayer;
using Almostengr.LightShowExtender.Infrastructure.FalconPiPlayer;
using Almostengr.LightShowExtender.Infrastructure.Wled;
using Almostengr.LightShowExtender.DomainService.Wled;
using Almostengr.Common.HomeAssistant;
using Almostengr.Common.NwsWeather;
using Almostengr.Common.TheAlmostEngineer;

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

        services.AddSingleton<IEngineerHttpClient, EngineerHttpClient>();
        services.AddSingleton<IFppHttpClient, FppHttpClient>();
        services.AddSingleton<IHomeAssistantHttpClient, HomeAssistantHttpClient>();
        services.AddSingleton<INwsHttpClient, NwsHttpClient>();
        services.AddSingleton<IWledHttpClient, WledHttpClient>();

        services.AddSingleton<IExtenderService, ExtenderService>();
        services.AddSingleton(typeof(ILoggingService<>), typeof(LoggingService<>));

        services.AddHostedService<WebsiteDisplayWorker>();
    })
    .UseSystemd()
    .Build();

await host.RunAsync();
