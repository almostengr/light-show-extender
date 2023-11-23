using System.Text;
using System.Text.Json.Serialization;
using Almostengr.Extensions;
using Almostengr.Extensions.Logging;
using Almostengr.LightShowExtender.DomainService.Common;
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
        IOptions<TwitterOptions> options,
        ILoggingService<PostTweetHandler> loggingService
        )
    {
        _loggingService = loggingService;
        _options = options;

        var credentials = new TwitterCredentials(
            _options.Value.ConsumerKey, _options.Value.ConsumerSecret,
            _options.Value.AccessToken, _options.Value.AccessSecret);

        _twitterClient = new TwitterClient(credentials);
        _twitterClient.Config.TweetMode = TweetMode.None;
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

            if (string.IsNullOrWhiteSpace(command.Title) && string.IsNullOrWhiteSpace(command.StatusChange))
            {
                throw new ArgumentNullException(nameof(command.Title));
            }

            StringBuilder tweet = new();

            if (command.Title.IsNotNullOrWhiteSpace())
            {
                tweet.Append($"Playing {command.Title} ");

                if (command.Aritst.IsNotNullOrWhiteSpace())
                {
                    tweet.Append($"by {command.Aritst} ");
                }

                tweet.Append($"at {DateTime.Now.TimeOfDay} ");
            }
            else
            {
                tweet.Append($"{command.StatusChange} ");
            }

            string hashTags = GetHashTags();
            tweet.Append(hashTags);

            // below code taken from https://github.com/linvi/tweetinvi/issues/1147
            TweetV2PostRequest tweetParams = new(tweet.ToString());

            await _twitterClient.Execute.AdvanceRequestAsync(
                (ITwitterRequest request) =>
                {
                    // var jsonBody = _twitterClient.Json.Serialize(tweetParams);
                    // var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                    var content = AeHttpClient.SerializeRequestBody<TweetV2PostRequest>(tweetParams);

                    request.Query.Url = "https://api.twitter.com/2/tweets";
                    request.Query.HttpMethod = Tweetinvi.Models.HttpMethod.POST;
                    request.Query.HttpContent = content;
                }
            );
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
            "#HappyHolidays", "#ChristmasMagic", "#ChristmasLighting", $"#Christmas{DateTime.Now.Year}"
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

    public sealed class TweetV2PostRequest
    {
        public TweetV2PostRequest(string text)
        {
            Text = text;
        }

        [JsonPropertyName("text")]
        public string Text { get; init; } = string.Empty;
    }
}

public sealed class PostTweetCommand : ICommand
{
    public PostTweetCommand(string title, string aritst)
    {
        Title = title;
        Aritst = aritst;
    }

    public PostTweetCommand(string statusChange)
    {
        StatusChange = statusChange;
    }

    public string Title { get; init; } = string.Empty;
    public string Aritst { get; init; } = string.Empty;
    public string StatusChange { get; init; } = string.Empty;
}
