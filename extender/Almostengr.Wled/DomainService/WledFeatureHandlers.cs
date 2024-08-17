using Almostengr.Wled.DomainService
using Microsoft.Extensions.DependencyInjection;

namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public static class WledFeatureHandlers 
{
    public static void AddHandlers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<TurnOffWledHandler>();
        serviceCollection.AddSingleton<TurnOnWledHandler>();
    }
}