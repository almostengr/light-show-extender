using Almostengr.LightShowExtender.DomainService.Wled;
using Microsoft.Extensions.DependencyInjection;

namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public static class WledFeatureHandlers 
{
    public static void AddHandlers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<TurnOffWledHandler>();
        serviceCollection.AddScoped<TurnOnWledHandler>();
    }
}