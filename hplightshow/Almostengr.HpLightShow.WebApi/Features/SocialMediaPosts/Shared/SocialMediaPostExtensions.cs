using Almostengr.HpLightShow.WebApi.Features.SocialMediaPosts.DomainServices;
using Almostengr.HpLightShow.WebApi.Features.SocialMediaPosts.DomainServices.Interfaces;
using Almostengr.HpLightShow.WebApi.Models;
using Tweetinvi;
using X.Bluesky;

namespace Almostengr.HpLightShow.WebApi.Features.SocialMediaPosts.Shared;

public static class SocialMediaPostExtensions
{
    public static void AddSocialMediaPostServices(this IServiceCollection services)
    {
        services.AddTransient<ISocialMediaPosterService, SocialMediaPosterService>();

        services.AddTransient<IBlueskyClient>(service =>
        {
            AppSettings appSettings = service.GetRequiredService<AppSettings>();
            return new BlueskyClient(appSettings.BlueSky.Username, appSettings.BlueSky.Password);
        });

        services.AddTransient<ITwitterClient>(service =>
        {
            AppSettings appSettings = service.GetRequiredService<AppSettings>();
            return new TwitterClient(
                appSettings.Twitter.ConsumerKey,
                appSettings.Twitter.ConsumerSecret,
                appSettings.Twitter.AccessToken,
                appSettings.Twitter.AccessSecret);
        });
    }
}