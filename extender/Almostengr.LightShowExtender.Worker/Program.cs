using Almostengr.LightShowExtender.Worker;
using Almostengr.NationalWeatherService.DomainService;
using Almostengr.NationalWeatherService.Infrastructure;
using Almostengr.FalconPiPlayer.DomainService;
using Almostengr.FalconPiPlayer.Infrastructure;

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

        services.AddSingleton<IFppHttpClient, FppHttpClient>();
        services.AddSingleton<INwsHttpClient, NwsHttpClient>();

        // services.AddSingleton(typeof(ILoggingService<>), typeof(LoggingService<>));

        services.AddHostedService<ExtenderWorker>();
    })
    .UseSystemd()
    .Build();

await host.RunAsync();
