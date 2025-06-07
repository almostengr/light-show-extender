using Almostengr.Common.DomainServices.Results;
using Almostengr.HpLightShow.WebApi.Features.SocialMediaPosts.DomainServices.Interfaces;
using Almostengr.HpLightShow.WebApi.Features.SocialMediaPosts.DomainServices.Resources;
using Tweetinvi;
using X.Bluesky;

namespace Almostengr.HpLightShow.WebApi.Features.SocialMediaPosts.DomainServices;

public sealed class SocialMediaPosterService : ISocialMediaPosterService
{
    private readonly IBlueskyClient _blueskyClient;
    private readonly ILogger<SocialMediaPosterService> _logger;
    private readonly ITwitterClient _twitterClient;

    public SocialMediaPosterService(
        IBlueskyClient blueskyClient,
        ILogger<SocialMediaPosterService> logger,
        ITwitterClient twitterClient
    )
    {
        _blueskyClient = blueskyClient;
        _logger = logger;
        _twitterClient = twitterClient;
    }

    public async Task<Result<SocialMediaResource>> ExecuteAsync(SocialMediaResource resource, bool commitTransaction = true)
    {
        if (resource == null)
        {
            return Result<SocialMediaResource>.Failure("Resource is null.");
        }

        if (string.IsNullOrWhiteSpace(resource.Text))
        {
            return Result<SocialMediaResource>.Failure("No message provided to post.");
        }

        var result = Result<SocialMediaResource>.Create();

        string postText = string.Concat(resource.Text, " ", DateTime.Now.Microsecond);

        // todo - include hash tags in the post text

        try
        {
            await _blueskyClient.Post(postText);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            result.AddError(ex.Message);
        }

        try
        {
            await _twitterClient.Tweets.PublishTweetAsync(postText);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            result.AddError(ex.Message);
        }

        return result;
    }

    public async Task<Result<SocialMediaResource>> PostAsync(string message)
    {
        SocialMediaResource resource = new();
        resource.Text = message;
        return await ExecuteAsync(resource);
    }
}
