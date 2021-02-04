using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiMonitor.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Almostengr.FalconPiMonitor
{
    public class TwitterRepliesService : BaseService
    {
        public TwitterRepliesService(ILogger<BaseService> logger, IConfiguration configuration) : base(logger, configuration)
        {
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
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