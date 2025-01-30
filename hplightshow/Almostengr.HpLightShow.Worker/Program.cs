using Almostengr.HpLightShow.Core.Common;
using Almostengr.HpLightShow.Core.Countdowns.Common;
using Almostengr.HpLightShow.Core.FalconPiPlayer.Common;
using Almostengr.HpLightShow.Core.FalconPiPlayer.Infrastructure;
using Almostengr.HpLightShow.Worker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHttpClient<IFppClient, FppClient>();

loadConfiguration(builder);

CountdownsBuilderService.Add(builder.Services);
FalconPiPlayerBuilderService.Add(builder.Services);

// builder.Services.AddSingleton(typeof(ILogger<>), typeof(LoggingService<>));

builder.Services.AddHostedService<CountdownWorker>();
builder.Services.AddHostedService<FppMonitorWorker>();

var host = builder.Build();
host.Run();




void loadConfiguration(HostApplicationBuilder builder)
{
    const string PROD = "prod";
    string environment = PROD;

#if !RELEASE
    environment = "devl";
#endif

    builder.Configuration.Sources.Clear();

    IConfiguration configuration = new ConfigurationBuilder()
        .AddJsonFile(
            (environment == PROD) ?
                "/home/fpp/media/upload/appsettings.json" :
                "appsettings.Development.json",
            false,
            false)
        .Build();

    // builder.Configuration.AddConfiguration(configuration);
    builder.Services.AddSingleton(configuration.GetSection(nameof(AppSettings)));
}