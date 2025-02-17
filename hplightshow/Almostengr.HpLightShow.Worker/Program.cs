using Almostengr.HpLightShow.Core.Countdowns.Shared;
using Almostengr.HpLightShow.Core.FalconPiPlayer.DomainServices.Infrastructure;
using Almostengr.HpLightShow.Core.FalconPiPlayer.Infrastructure;
using Almostengr.HpLightShow.Core.FalconPiPlayer.Shared;
using Almostengr.HpLightShow.Core.Wled.DomainServices.Interfaces;
using Almostengr.HpLightShow.Core.Wled.Infrastructure;
using Almostengr.HpLightShow.Worker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHttpClient<IFppClient, FppClient>();
builder.Services.AddHttpClient<IWledClient, WledClient>();

loadConfiguration(builder);

CountdownDependencyInjection.Add(builder.Services);
FalconPiPlayerDependencyInjection.Add(builder.Services);

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

    builder.Services.AddSingleton(configuration.GetSection(nameof(CountdownAppSettings)));
    builder.Services.AddSingleton(configuration.GetSection(nameof(FppAppSettings)));
}