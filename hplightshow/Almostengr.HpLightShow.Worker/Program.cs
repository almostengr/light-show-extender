using Almostengr.HpLightShow.Worker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddTransient<ChristmasCountdownHandler>();
builder.Services.AddTransient<FppMonitorHandler>();

builder.Services.AddHostedService<ChristmasCountdownWorker>();
builder.Services.AddHostedService<FppMonitorWorker>();

var host = builder.Build();
host.Run();
