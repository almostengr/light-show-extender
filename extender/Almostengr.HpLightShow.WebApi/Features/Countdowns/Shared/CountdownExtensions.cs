using Almostengr.HpLightShow.Core.Countdowns.Service;
using Almostengr.HpLightShow.WebApi.Countdowns.DomainServices.Interfaces;

namespace Almostengr.HpLightShow.WebApi.Features.Countdowns.Shared;

public static class CountdownExtensions
{
    public static void AddCountdownServices(this IServiceCollection services)
    {
        services.AddTransient<ICountdownService, CountdownService>();
    }
}
