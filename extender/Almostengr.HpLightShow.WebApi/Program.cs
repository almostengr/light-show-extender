using Almostengr.HpLightShow.WebApi.Features.Countdowns.Shared;
using Almostengr.HpLightShow.WebApi.Features.Monitoring.Shared;
using Almostengr.HpLightShow.WebApi.Features.SocialMediaPosts.Shared;
using Almostengr.HpLightShow.WebApi.Features.StartSequence.Shared;
using Almostengr.HpLightShow.WebApi.Models;
using Almostengr.HpLightShow.WebApi.Workers;

var builder = WebApplication.CreateBuilder(args);

loadConfiguration(builder);

builder.Services.AddCountdownServices();
builder.Services.AddMonitorServices();
builder.Services.AddSocialMediaPostServices();
builder.Services.AddStartSequenceServices();

builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

// builder.Services.AddHostedService<CountdownWorker>();
builder.Services.AddHostedService<MonitorWorker>();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();


void loadConfiguration(WebApplicationBuilder builder)
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
                "/home/fpp/media/upload/appsettings.json" : "appsettings.Development.json",
            false,
            false)
        .Build();

    builder.Services.AddSingleton(configuration.GetSection(nameof(AppSettings)));
}
