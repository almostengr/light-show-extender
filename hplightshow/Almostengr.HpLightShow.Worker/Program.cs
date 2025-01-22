using Almostengr.HpLightShow.Core.Countdowns;
using Almostengr.HpLightShow.Worker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHttpClient();

builder.Services.AddTransient<ICountdownService, CountdownService>();

builder.Services.AddHostedService<CountdownWorker>();
builder.Services.AddHostedService<FppMonitorWorker>();

var host = builder.Build();
host.Run();
