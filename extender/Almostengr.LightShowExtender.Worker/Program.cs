using Almostengr.LightShowExtender.DomainService.TweetInvi;
using Almostengr.LightShowExtender.Worker;
using Almostengr.Extensions.Logging;
using Almostengr.LightShowExtender.DomainService.Common;

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

        services.Configure<TwitterOptions>(configuration.GetSection(nameof(TwitterOptions)));
        TwitterOptions twitterOptions = configuration.GetSection(nameof(TwitterOptions)).Get<TwitterOptions>();

        services.AddSingleton<IFppHttpClient, FppHttpClient>();
        services.AddSingleton<IWledHttpClient, WledHttpClient>();

        TwitterFeatureHandler.AddHandlers(services);

        services.AddSingleton(typeof(ILoggingService<>), typeof(LoggingService<>));

        services.AddHostedService<ExtenderWorker>();
    })
    .UseSystemd()
    .Build();

await host.RunAsync();
