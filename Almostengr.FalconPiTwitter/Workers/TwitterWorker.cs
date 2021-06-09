using System;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.Models;
using Microsoft.Extensions.Logging;
using Tweetinvi;

namespace Almostengr.FalconPiTwitter.Workers
{
    public class TwitterWorker : BaseWorker, ITwitterWorker
    {
        private readonly ILogger<TwitterWorker> _logger;
        private readonly ITwitterClient _twitterClient;

        public TwitterWorker(ILogger<TwitterWorker> logger, AppSettings appSettings, ITwitterClient twitterClient) :
             base(logger, appSettings, twitterClient)
        {
            _logger = logger;
            _twitterClient = twitterClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await LikeMentionedTweets();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, string.Concat(ex.GetType(), ex.Message));
                }

                await Task.Delay(TimeSpan.FromMinutes(15));
            }
        }

        public async Task LikeMentionedTweets()
        {
            var mentions = await _twitterClient.Timelines.GetMentionsTimelineAsync();

            foreach (var mention in mentions)
            {
                if (mention.Favorited == false)
                {
                    await mention.FavoriteAsync();
                    _logger.LogInformation("Favorited tweet: " + mention.Id);
                }
            }
        }
    }
}