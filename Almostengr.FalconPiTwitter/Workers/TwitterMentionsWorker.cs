using System;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.Constants;
using Almostengr.FalconPiTwitter.Settings;
using Microsoft.Extensions.Logging;
using Tweetinvi;

namespace Almostengr.FalconPiTwitter.Workers
{
    public class TwitterMentionsWorker : BaseWorker, ITwitterMentionsWorker
    {
        private readonly ILogger<TwitterMentionsWorker> _logger;
        private readonly ITwitterClient _twitterClient;

        public TwitterMentionsWorker(ILogger<TwitterMentionsWorker> logger, AppSettings appSettings, ITwitterClient twitterClient) :
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
                    await ProcessMentionedTweets();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }

                await Task.Delay(TimeSpan.FromSeconds(DelaySeconds.ExtraLong), stoppingToken);
            }
        }

        public async Task ProcessMentionedTweets()
        {
            _logger.LogInformation("Checking for mentioned tweets");
            var mentions = await _twitterClient.Timelines.GetMentionsTimelineAsync();

            foreach (var mention in mentions)
            {
                if (mention.Favorited == false)
                {
                    await mention.FavoriteAsync();
                }
            }
        }
    }
}