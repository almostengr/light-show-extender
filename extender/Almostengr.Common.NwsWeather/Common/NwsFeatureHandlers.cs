using Microsoft.Extensions.DependencyInjection;

namespace Almostengr.Common.NwsWeather.Common;

public static class NwsFeatureHandlers
{
    public static void AddHandlers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<GetLatestObservationHandler>();
    }
}