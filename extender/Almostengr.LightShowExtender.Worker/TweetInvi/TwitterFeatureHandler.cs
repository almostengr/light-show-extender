using Microsoft.Extensions.DependencyInjection;

namespace Almostengr.LightShowExtender.DomainService.TweetInvi;

public static class TwitterFeatureHandler{
    public static void AddHandlers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<PostTweetHandler>();
    }
}