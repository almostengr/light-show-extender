using Almostengr.Extensions;

namespace Almostengr.Common.NwsWeather.Common;

public static class NwsFeatureHandlers : IFeatureHandlers
{
    public static void AddHandlers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<GetLatestObservationHandler>();
    }
}