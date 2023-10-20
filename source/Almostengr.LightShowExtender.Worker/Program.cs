using Almostengr.LightShowExtender.DomainService;
using Almostengr.LightShowExtender.Worker;
using Almostengr.LightShowExtender.DomainService.Common;
using Almostengr.Common.Logging;
using Almostengr.LightShowExtender.DomainService.Common.Constants;
using Almostengr.LightShowExtender.Infrastructure.Wled;
using Almostengr.LightShowExtender.DomainService.Wled;
using Almostengr.Common.HomeAssistant;
using Almostengr.Common.NwsWeather;
using Almostengr.Common.TheAlmostEngineer;
using Almostengr.LightShowExtender.Infrastructure.FalconPiPlayer;
using Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

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

        services.Configure<HomeAssistantOptions>(configuration.GetSection("HomeAssistant"));
        services.AddHttpClient<IHomeAssistantHttpClient, HomeAssistantHttpClient>();
        services.AddSingleton<IHomeAssistantService, HomeAssistantService>();
        
        services.Configure<LightShowOptions>(configuration.GetSection("TheAlmostEngineer"));
        services.AddHttpClient<ILightShowHttpClient, LightShowHttpClient>();
        services.AddSingleton<ILightShowService, LightShowService>();

        services.Configure<NwsOptions>(configuration.GetSection("NationalWeatherService"));
        services.AddHttpClient<INwsHttpClient, NwsHttpClient>();
        services.AddSingleton<INwsService, NwsService>();

        services.AddHttpClient<IFppHttpClient, FppHttpClient>();
        services.AddSingleton<IFppService, FppService>();

        services.AddHttpClient<IWledHttpClient, WledHttpClient>();

        services.AddSingleton<IExtenderService, ExtenderService>();
        services.AddSingleton(typeof(ILoggingService<>), typeof(LoggingService<>));

        services.AddHostedService<WebsiteDisplayWorker>();
    })
    .UseSystemd()
    .Build();

await host.RunAsync();
