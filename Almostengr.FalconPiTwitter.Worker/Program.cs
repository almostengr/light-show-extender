using Almostengr.FalconPiTwitter.Worker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<ChristmasCountDownWorker>();
        services.AddHostedService<FppCurrentSongWorker>();
        services.AddHostedService<FppVitalsWorker>();
        services.AddHostedService<LightShowCountdownWorker>();
    })
    .Build();

await host.RunAsync();
