using Almostengr.Common.Extensions;
using Almostengr.FalconPiPlayerClient.DomainServices.Interfaces;
using Almostengr.FalconPiPlayerClient.Infrastructure;
using Almostengr.HpLightShow.Core.Countdowns.Shared;
using Almostengr.HpLightShow.Core.FalconPiPlayer.Shared;
using Almostengr.HpLightShow.Worker;
using Almostengr.WledClient.DomainServices.Interfaces;
using Almostengr.WledClient.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHttpClient<IFppdHttpClient,FppdClient>();
builder.Services.AddHttpClient<IWledClient, WledClient>();

loadConfiguration(builder);

CommonDependencyInjection.AddServices(builder.Services);
CountdownDependencyInjection.AddServices(builder.Services);
FalconPiPlayerDependencyInjection.AddServices(builder.Services);

builder.Services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

// builder.Services.AddHostedService<CountdownWorker>();
// builder.Services.AddHostedService<FppMonitorWorker>();

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