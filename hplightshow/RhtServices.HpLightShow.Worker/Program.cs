using RhtServices.HpLightShow.Core.DomainHandler.ChristmasCountdown;

namespace RhtServices.HpLightShow.Worker;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddTransient<ChristmasCountdownHandler>();
        builder.Services.AddHostedService<ChristmasCountdownWorker>();

        var host = builder.Build();
        host.Run();
    }
}