using System.Text;
using System.Text.Json.Serialization;
using Almostengr.Extensions;
using Almostengr.LightShowExtender.Worker;
using Tweetinvi;
using Tweetinvi.Models;

namespace Almostengr.LightShowExtender.DomainService.Twitter;

public sealed class PostTweetHandler : ICommandHandler<PostUpdateCommand>
{
    private readonly ITwitterClient _twitterClient;
    private readonly TwitterSettings _twitterSettings;

    public PostTweetHandler(TwitterSettings twitterSettings)
    {
        _twitterSettings = twitterSettings;

        TwitterCredentials credentials = new TwitterCredentials(
            _twitterSettings.ConsumerKey, _twitterSettings.ConsumerSecret,
            _twitterSettings.AccessToken, _twitterSettings.AccessSecret);

        _twitterClient = new TwitterClient(credentials);
        _twitterClient.Config.TweetMode = TweetMode.None;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken, PostUpdateCommand command)
    {
        if (_twitterClient == null)
        {
            throw new ArgumentException("Twitter client not configured");
        }

        if (command == null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        if (string.IsNullOrWhiteSpace(command.Text))
        {
            throw new ArgumentNullException(nameof(command.Text));
        }

        StringBuilder tweet = new(command.Text);
        // StringBuilder tweet = new();

        // if (command.Text.IsNotNullOrWhiteSpace())
        // {
        //     tweet.Append($"Playing {command.Text} ");

        //     if (command.Aritst.IsNotNullOrWhiteSpace())
        //     {
        //         tweet.Append($"by {command.Aritst} ");
        //     }

        //     tweet.Append($"at {DateTime.Now.TimeOfDay} ");
        // }
        // else
        // {
        //     tweet.Append($"{command.StatusChange} ");
        // }

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

    private string GetHashTags()
    {
        string[] hashTags = {
            "#ChristmasLights", "#LightShow", "#HolidayLightShows", "#HolidayLights",
            "#HappyHolidays", "#ChristmasMagic", "#ChristmasLighting", $"#Christmas{DateTime.Now.Year}"
        };

        Random random = new();
        StringBuilder tags = new();
        uint numberOfTags = 0;

        while (numberOfTags < _twitterSettings.HashTagCount)
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
