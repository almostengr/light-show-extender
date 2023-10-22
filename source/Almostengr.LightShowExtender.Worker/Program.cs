using Almostengr.LightShowExtender.Worker;
using Almostengr.Common.Logging;
using Almostengr.Common.HomeAssistant;
using Almostengr.Common.NwsWeather;
using Almostengr.Common.TheAlmostEngineer;
using Almostengr.LightShowExtender.DomainService.Common;
using Almostengr.LightShowExtender.DomainService.FalconPiPlayer;
using Almostengr.LightShowExtender.Infrastructure.FalconPiPlayer;
using Almostengr.LightShowExtender.Infrastructure.Wled;
using Almostengr.LightShowExtender.DomainService.Wled;
using Almostengr.LightShowExtender.DomainService;

Console.WriteLine(typeof(Program).Assembly.ToString());

const string PROD = "prod";
string environment = PROD;

#if !RELEASE
environment = "devl";
#endif

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile(
        (environment == PROD) ? "/home/fpp/media/upload/lightshowextender.appsettings.json" : "appsettings.Development.json",
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

        services.Configure<HomeAssistantOptions>(configuration.GetSection(nameof(HomeAssistantOptions)));
        services.AddHttpClient<IHomeAssistantHttpClient, HomeAssistantHttpClient>();
        services.AddSingleton<IHomeAssistantService, HomeAssistantService>();

        services.Configure<LightShowOptions>(configuration.GetSection(nameof(LightShowOptions)));
        services.AddHttpClient<ILightShowHttpClient, LightShowHttpClient>();
        services.AddSingleton<ILightShowService, LightShowService>();

        services.Configure<NwsOptions>(configuration.GetSection(nameof(NwsOptions)));
        services.AddHttpClient<INwsHttpClient, NwsHttpClient>();
        services.AddSingleton<INwsService, NwsService>();

        services.AddHttpClient<IFppHttpClient, FppHttpClient>();
        services.AddSingleton<IFppService, FppService>();

        services.AddHttpClient<IWledHttpClient, WledHttpClient>();
        services.AddSingleton<IWledService, WledService>();

        services.AddSingleton<IExtenderService, ExtenderService>();
        
        services.AddSingleton(typeof(ILoggingService<>), typeof(LoggingService<>));

        services.AddHostedService<WebsiteDisplayWorker>();
    })
    .UseSystemd()
    .Build();

await host.RunAsync();
