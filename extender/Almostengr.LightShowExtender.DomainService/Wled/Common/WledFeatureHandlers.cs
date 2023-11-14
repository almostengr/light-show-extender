namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public static class WledFeatureHandlers : IFeatureHandlers
{
    public static void AddHandlers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<TurnOffHandler>();
        serviceCollection.AddScoped<TurnOnHandler>();
    }
}