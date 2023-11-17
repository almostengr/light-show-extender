using Almostengr.LightShowExtender.DomainService.Website;
using Microsoft.Extensions.DependencyInjection;

namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public static class WebsiteFeatureHandlers
{
    public static void AddHandlers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<DeleteSongsInQueueHandler>();
        serviceCollection.AddSingleton<GetNextSongInQueueHandler>();
        serviceCollection.AddSingleton<PostDisplayInfoHandler>();        
    }
}