using System.Text;
using System.Text.Json.Serialization;
using Almostengr.Common;
using Almostengr.Common.Command;
using Almostengr.LightShowExtender.Worker;
using Almostengr.LightShowExtender.Worker.Twitter;
using Tweetinvi;
using Tweetinvi.Models;

namespace Almostengr.LightShowExtender.DomainService.Twitter;

public sealed class PostTweetCommandHandler : ICommandHandler<PostTweetCommand, PostTweetResponse>
{
    private readonly ITwitterClient _twitterClient;
    private readonly TwitterAppSettings _twitterSettings;

    public PostTweetCommandHandler(TwitterAppSettings twitterSettings)
    {
        _twitterSettings = twitterSettings;

        TwitterCredentials credentials = new TwitterCredentials(
            _twitterSettings.ConsumerKey, _twitterSettings.ConsumerSecret,
            _twitterSettings.AccessToken, _twitterSettings.AccessSecret);

        _twitterClient = new TwitterClient(credentials);
        _twitterClient.Config.TweetMode = TweetMode.None;
    }

    public async Task<PostTweetResponse> ExecuteAsync(CancellationToken cancellationToken, PostTweetCommand command)
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

        string hashTags = GetHashTags();
        tweet.Append(hashTags);

        // below code taken from https://github.com/linvi/tweetinvi/issues/1147
        TweetV2PostRequest tweetParams = new(tweet.ToString());

        Tweetinvi.Core.Web.ITwitterResult result = await _twitterClient.Execute.AdvanceRequestAsync(
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

        if (result.Response.IsSuccessStatusCode)
        {
            return new PostTweetResponse(true);
        }

        return new PostTweetResponse(false);
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
