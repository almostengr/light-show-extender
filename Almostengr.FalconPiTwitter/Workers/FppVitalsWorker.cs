using System;
using System.Collections.Generic;
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
    public class FppVitalsWorker : BaseWorker
    {
        private readonly ILogger<FppVitalsWorker> _logger;
        private readonly ITwitterService _twitterService;
        private readonly IFppService _fppService;

        public FppVitalsWorker(ILogger<FppVitalsWorker> logger, AppSettings appSettings, IServiceScopeFactory factory)
             : base(logger)
        {
            _logger = logger;
            _fppService = factory.CreateScope().ServiceProvider.GetRequiredService<IFppService>();
            _twitterService = factory.CreateScope().ServiceProvider.GetRequiredService<ITwitterService>();
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting vitals worker");

            string previousSecondsPlayed = string.Empty,
                previousSecondsRemaining = string.Empty;

            await _twitterService.GetAuthenticatedUserAsync();

            List<string> fppHosts = await _fppService.GetFppHostsAsync();
            int alarmCount = 0;

            while (!stoppingToken.IsCancellationRequested)
            {
                if (DateTime.Now.Minute >= 54 && alarmCount > 0)
                {
                    _logger.LogInformation("Resetting alarm count");
                    alarmCount = 0;
                }

                foreach (var fppInstance in fppHosts)
                {
                    _logger.LogInformation($"Checking vitals for {fppInstance}");

                    FalconFppdStatusDto falconFppdStatus = await _fppService.GetFppdStatusAsync(fppInstance);

                    if (falconFppdStatus == null)
                    {
                        await TaskDelayAsync(DelaySeconds.Smedium, stoppingToken);
                        continue;
                    }

                    foreach (var sensor in falconFppdStatus.Sensors)
                    {
                        string alarmMessage = _fppService.CheckSensorData(sensor);
                        await _twitterService.PostTweetAlarmAsync(alarmMessage, alarmCount);

                        if (alarmMessage != string.Empty)
                        {
                            alarmCount++;
                        }
                    }

                    bool IsFppStuck = _fppService.CheckForStuckFpp(previousSecondsPlayed, previousSecondsRemaining, falconFppdStatus);

                    if (IsFppStuck)
                    {
                        var task = Task.Run(() => _twitterService.PostTweetAlarmAsync(ExceptionMessage.FppFrozen, alarmCount));
                        alarmCount++;
                    }

                    previousSecondsPlayed = falconFppdStatus.Seconds_Played;
                    previousSecondsRemaining = falconFppdStatus.Seconds_Remaining;
                }

                await TaskDelayAsync(DelaySeconds.Medium, stoppingToken);
            }
        }

    }
}