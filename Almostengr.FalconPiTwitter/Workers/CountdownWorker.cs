using System;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.Constants;
using Almostengr.FalconPiTwitter.DataTransferObjects;
using Almostengr.FalconPiTwitter.Services;
using Almostengr.FalconPiTwitter.Settings;
using Microsoft.Extensions.Logging;

namespace Almostengr.FalconPiTwitter.Workers
{
    public class CountdownWorker : BaseWorker
    {
        private readonly ILogger<CountdownWorker> _logger;
        private readonly IFppService _fppService;
        private readonly ITwitterService _twitterService;

        public CountdownWorker(ILogger<CountdownWorker> logger, AppSettings appSettings,
            ITwitterService twitterService, IFppService fppService) :
            base(logger, appSettings)
        {
            _logger = logger;
            _fppService = fppService;
            _twitterService = twitterService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting countdown worker");
            
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    DateTime currentDateTime = DateTime.Now;
                    string tweetString = string.Empty;
                    FalconFppdStatusDto status = await _fppService.GetFppdStatusAsync(AppConstants.Localhost);

                    if (status == null)
                    {
                        _logger.LogError("Fpp did not respond. Is it online?");
                        await Task.Delay(TimeSpan.FromSeconds(DelaySeconds.Short), stoppingToken);
                        break;
                    }

                    tweetString += _fppService.TimeUntilNextLightShow(currentDateTime, status.Next_Playlist.Start_Time);

                    if (_fppService.IsPlaylistIdleOfflineOrTesting(status))
                    {
                        tweetString += _fppService.TimeUntilChristmas(currentDateTime);
                        tweetString += _twitterService.GetRandomChristmasHashTag();

                        await _twitterService.PostTweetAsync(tweetString);
                    }

                    await Task.Delay(TimeSpan.FromHours(_fppService.GetRandomWaitTime()), stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    await Task.Delay(TimeSpan.FromSeconds(DelaySeconds.Short), stoppingToken);
                }

            }
        }

    }
}