using System;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.Common;
using Almostengr.FalconPiTwitter.Common.Constants;
using Almostengr.FalconPiTwitter.DataTransferObjects;
using Almostengr.FalconPiTwitter.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tweetinvi.Exceptions;

namespace Almostengr.FalconPiTwitter.Workers
{
    public class CountdownWorker : BackgroundService
    {
        private readonly ILogger<CountdownWorker> _logger;
        private readonly IFppService _fppService;
        private readonly AppSettings _appSettings;

        public CountdownWorker(ILogger<CountdownWorker> logger, AppSettings appSettings, IFppService fppService)
        {
            _logger = logger;
            _fppService = fppService;
            _appSettings = appSettings;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    DateTime currentDateTime = DateTime.Now;
                    FalconFppdStatusDto status = await _fppService.GetFppdStatusAsync(_appSettings.FppHosts[0]);

                    if (status == null)
                    {
                        _logger.LogError("Fpp did not respond. Is it online?");
                        await Task.Delay(TimeSpan.FromSeconds(DelaySeconds.Short), stoppingToken);
                        continue;
                    }

                    string tweetString = _fppService.TimeUntilNextLightShow(currentDateTime, status.Next_Playlist.Start_Time);

                    await _fppService.PostChristmasCountDownWhenIdleAsync(status);

                    await Task.Delay(TimeSpan.FromHours(_fppService.GetRandomWaitTime()), stoppingToken);
                }
                catch (TwitterException ex)
                {
                    _logger.LogError(ex, ex.Message);
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    await Task.Delay(TimeSpan.FromSeconds(DelaySeconds.Medium), stoppingToken);
                }

            }
        }

    }
}