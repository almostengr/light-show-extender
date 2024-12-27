using RhtServices.FalconPiPlayer.DomainService;
using RhtServices.FalconPiPlayer.Infrastructure;
using RhtServices.HpLightShow.Core.DomainHandler.ChristmasCountdown;
using RhtServices.HpLightShow.Core.DomainHandler.FppMonitor;

namespace RhtServices.HpLightShow.Worker;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddSingleton<IFppHttpClient, FppHttpClient>();

        builder.Services.AddTransient<ChristmasCountdownHandler>();
        builder.Services.AddTransient<FppMonitorHandler>();

        builder.Services.AddHostedService<ChristmasCountdownWorker>();
        builder.Services.AddHostedService<FppMonitorWorker>();

        var host = builder.Build();
        host.Run();
    }
}