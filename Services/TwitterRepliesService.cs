using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiMonitor.ServicesBase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Almostengr.FalconPiMonitor
{
    public class TwitterRepliesService : BaseService
    {
        public TwitterRepliesService(ILogger<BaseService> logger, IConfiguration configuration) : base(logger, configuration)
        {
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // don't run if values are not set or action is disabled

            while (!stoppingToken.IsCancellationRequested)
            {
                Tweetinvi.Models.ITweet[] mentions = await TwitterClient.Timelines.GetMentionsTimelineAsync();

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
}