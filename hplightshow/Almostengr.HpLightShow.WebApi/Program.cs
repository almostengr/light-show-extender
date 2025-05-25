using Almostengr.Common.Extensions;
using Almostengr.FalconPiPlayerClient.DomainServices.Interfaces;
using Almostengr.FalconPiPlayerClient.Infrastructure;
using Almostengr.HpLightShow.Core.FalconPiPlayer.Shared;
using Almostengr.HpLightShow.WebApi.Features.Wled.DomainServices.Interfaces;
using Almostengr.HpLightShow.WebApi.Features.Wled.Infrastructure;
using Almostengr.HpLightShow.WebApi.Features.Wled.Shared;
using Almostengr.HpLightShow.WebApi.Workers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<IFppdHttpClient, FppdClient>(
    options => options.BaseAddress = new Uri("http://10.10.50.101")
);
builder.Services.AddHttpClient<IWledClient, WledClient>();

loadConfiguration(builder);

builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

CommonDependencyInjection.AddServices(builder.Services);
// CountdownDependencyInjection.AddServices(builder.Services);
FalconPiPlayerDependencyInjection.AddServices(builder.Services);
WledDependencyInjection.AddServices(builder.Services);
builder.Services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

builder.Services.AddHostedService<FppMonitorWorker>();

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



// void loadConfiguration(HostApplicationBuilder builder)
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

    // builder.Services.AddSingleton(configuration.GetSection(nameof(CountdownAppSettings)));
    // builder.Services.AddSingleton(configuration.GetSection(nameof(FppAppSettings)));
}