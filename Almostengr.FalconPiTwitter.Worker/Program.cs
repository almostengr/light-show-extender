using Almostengr.FalconPiTwitter.Worker;

IHost host = Host.CreateDefaultBuilder(args)
    .UseSystemd()
    .UseContentRoot(
        System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location))
    .ConfigureServices(services =>
    {
                    // IConfiguration configuration = hostContext.Configuration;
                    // services.Configuration()

        services.AddHostedService<ChristmasCountDownWorker>();
        services.AddHostedService<FppCurrentSongWorker>();
        services.AddHostedService<FppVitalsWorker>();
        services.AddHostedService<LightShowCountdownWorker>();

        
    })
    .Build();
    
Console.WriteLine(typeof(Program).Assembly.ToString());

await host.RunAsync();
