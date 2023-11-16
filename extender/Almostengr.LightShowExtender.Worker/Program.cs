using Almostengr.LightShowExtender.Worker;
using Almostengr.Extensions.Logging;
using Almostengr.Common.HomeAssistant.Common;
using Almostengr.Common.NwsWeather;
using Almostengr.Common.NwsWeather.Common;
using Almostengr.LightShowExtender.DomainService.Common;
using Almostengr.LightShowExtender.DomainService.FalconPiPlayer;
using Almostengr.LightShowExtender.Infrastructure.FalconPiPlayer;
using Almostengr.LightShowExtender.Infrastructure.Wled;
using Almostengr.LightShowExtender.DomainService.Wled;
using Almostengr.LightShowExtender.DomainService.Website.Common;
using Almostengr.LightShowExtender.Infrastructure.Website;

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
        services.AddSingleton<IHomeAssistantHttpClient, HomeAssistantHttpClient>();

        services.Configure<NwsOptions>(configuration.GetSection(nameof(NwsOptions)));
        services.AddSingleton<INwsHttpClient, NwsHttpClient>();

        services.Configure<WebsiteOptions>(configuration.GetSection(nameof(WebsiteOptions)));
        services.AddSingleton<IWebsiteHttpClient, WebsiteHttpClient>();

        services.AddSingleton<IFppHttpClient, FppHttpClient>();
        services.AddSingleton<IWledHttpClient, WledHttpClient>();

        WebsiteFeatureHandlers.AddHandlers(services);
        FppFeatureHandlers.AddHandlers(services);
        HomeAssistantFeatureHandlers.AddHandlers(services);
        NwsFeatureHandlers.AddHandlers(services);
        WledFeatureHandlers.AddHandlers(services);

        services.AddSingleton(typeof(ILoggingService<>), typeof(LoggingService<>));

        services.AddHostedService<ExtenderWorker>();
    })
    .UseSystemd()
    .Build();

await host.RunAsync();
