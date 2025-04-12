using Almostengr.Common.Extensions;
using Almostengr.FalconPiPlayerClient.DomainServices.Interfaces;
using Almostengr.FalconPiPlayerClient.Infrastructure;
using Almostengr.HpLightShow.Core.FalconPiPlayer.Shared;
using Almostengr.WledClient.DomainServices.Interfaces;
using Almostengr.WledClient.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<IFppdHttpClient, FppdClient>();
builder.Services.AddHttpClient<IWledClient, WledClient>();

builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

CommonDependencyInjection.AddServices(builder.Services);
// CountdownDependencyInjection.AddServices(builder.Services);
FalconPiPlayerDependencyInjection.AddServices(builder.Services);
builder.Services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

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
