using System.Text;
using Almostengr.Extensions;
using Almostengr.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tweetinvi;
using Tweetinvi.Models;

namespace Almostengr.LightShowExtender.DomainService.TweetInvi;

public sealed class PostTweetHandler : ICommandHandler<PostTweetCommand>
{
    private readonly ITwitterClient _twitterClient;
    private readonly ILoggingService<PostTweetHandler> _loggingService;
    private readonly IOptions<TwitterOptions> _options;

    public PostTweetHandler(
        ITwitterClient twitterClient,
        IOptions<TwitterOptions> options,
        ILoggingService<PostTweetHandler> loggingService
        )
    {
        _loggingService = loggingService;
        _options = options;

        var credentials = new TwitterCredentials(
            _options.Value.ConsumerKey, _options.Value.ConsumerSecret,
            _options.Value.AccessToken, _options.Value.AccessSecret);

        _twitterClient = twitterClient == null ? new TwitterClient(credentials) : twitterClient;
    }

    public async Task ExecuteAsync(PostTweetCommand command, CancellationToken cancellationToken)
    {
        try
        {
            if (_twitterClient == null)
            {
                throw new Exception("Twitter client not configured");
            }

            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (string.IsNullOrWhiteSpace(command.Title))
            {
                throw new ArgumentNullException(nameof(command.Title));
            }

            StringBuilder tweet = new($"Playing {command.Title} ");

            if (!string.IsNullOrWhiteSpace(command.Aritst))
            {
                tweet.Append($"by {command.Aritst} ");
            }

            tweet.Append($"at {DateTime.Now.TimeOfDay} ");

            string hashTags = GetHashTags();
            tweet.Append(hashTags);

            await _twitterClient.Tweets.PublishTweetAsync(tweet.ToString());
        }
        catch (Exception ex)
        {
            _loggingService.Error(ex.Message);
        }
    }

    private string GetHashTags()
    {
        string[] hashTags = {
            "#ChristmasLights", "#LightShow", "#HolidayLightShows", "#HolidayLights",
            "#HappyHolidays", "#ChristmasMagic", "#ChristmasLighting"
        };

        Random random = new();
        StringBuilder tags = new();
        uint numberOfTags = 0;

        while (numberOfTags < _options.Value.HashTagCount)
        {
            string tag = hashTags[random.Next(0, hashTags.Count())];
            if (!tags.ToString().Contains(tag))
            {
                tags.Append($"{tag} ");
                numberOfTags++;
            }
        }

        return tags.ToString();
    }
}

public sealed class PostTweetCommand : ICommand
{
    public PostTweetCommand(string title, string aritst)
    {
        Title = title;
        Aritst = aritst;
    }

    public string Title { get; init; } = string.Empty;
    public string Aritst { get; init; } = string.Empty;
}