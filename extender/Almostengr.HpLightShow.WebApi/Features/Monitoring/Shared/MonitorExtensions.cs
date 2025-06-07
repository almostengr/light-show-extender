using Almostengr.HpLightShow.WebApi.Features.Monitoring.DomainServices.Interfaces;
using Almostengr.HpLightShow.WebApi.Features.Monitoring.DomainServices.Resources;
using Almostengr.HpLightShow.WebApi.Features.Monitoring.Infrastructure;

namespace Almostengr.HpLightShow.WebApi.Features.Monitoring.Shared;

public static class MonitorExtensions
{
    public static void AddMonitorServices(this IServiceCollection services, string baseAddress = "http://127.0.0.1")
    {

        services.AddHttpClient<IFppdHttpClient, FppdClient>(client =>
        {
            client.BaseAddress = new Uri(baseAddress);
            client.Timeout = TimeSpan.FromSeconds(15);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        services.AddHttpClient<IWledClient, WledClient>(client =>
        {
            // client.BaseAddress = new Uri(baseAddress);
            client.Timeout = TimeSpan.FromSeconds(15);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        services.AddTransient<IMonitorService, MonitorService>();
    }
}
