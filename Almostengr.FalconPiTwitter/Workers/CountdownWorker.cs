using System;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.Constants;
using Almostengr.FalconPiTwitter.DataTransferObjects;
using Almostengr.FalconPiTwitter.Services;
using Almostengr.FalconPiTwitter.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Almostengr.FalconPiTwitter.Workers
{
    public class CountdownWorker : BaseWorker
    {
        private readonly ILogger<CountdownWorker> _logger;
        private readonly IFppService _fppService;
        private readonly ITwitterService _twitterService;

        public CountdownWorker(ILogger<CountdownWorker> logger, AppSettings appSettings,
            IServiceScopeFactory factory) : base(logger)
        {
            _logger = logger;
            _fppService = factory.CreateScope().ServiceProvider.GetRequiredService<IFppService>();
            _twitterService = factory.CreateScope().ServiceProvider.GetRequiredService<ITwitterService>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting countdown worker");

            while (!stoppingToken.IsCancellationRequested)
            {
                DateTime currentDateTime = DateTime.Now;
                string tweetString = string.Empty;
                FalconFppdStatusDto status = await _fppService.GetFppdStatusAsync(AppConstants.Localhost);

                if (status == null)
                {
                    _logger.LogError(ExceptionMessage.FppOffline);
                    await TaskDelayAsync(DelaySeconds.Smedium, stoppingToken);
                    continue;
                }

                tweetString += _fppService.TimeUntilNextLightShow(currentDateTime, status.Next_Playlist.Start_Time);

                if (_fppService.IsPlaylistIdleOfflineOrTesting(status))
                {
                    tweetString += _fppService.TimeUntilChristmas(currentDateTime);
                    tweetString += _twitterService.GetRandomChristmasHashTag();

                    await _twitterService.PostTweetAsync(tweetString);
                }

                double timeSeconds = _fppService.GetRandomWaitTime();
                await TaskDelayAsync(timeSeconds, stoppingToken);
            }
        }

    }
}