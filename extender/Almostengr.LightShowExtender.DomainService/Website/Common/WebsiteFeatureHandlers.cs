namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public static class WebsiteFeatureHandlers : IFeatureHandlers
{
    public static void AddHandlers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<DeleteSongsInQueueHandler>();
        serviceCollection.AddScoped<GetNextSongsInQueueHandler>();
        serviceCollection.AddScoped<PostDisplayInfoHandler>();        
    }
}