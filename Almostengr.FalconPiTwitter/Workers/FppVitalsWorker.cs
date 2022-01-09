using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.Constants;
using Almostengr.FalconPiTwitter.DataTransferObjects;
using Almostengr.FalconPiTwitter.Services;
using Almostengr.FalconPiTwitter.Settings;
using Microsoft.Extensions.Logging;

namespace Almostengr.FalconPiTwitter.Workers
{
    public class FppVitalsWorker : BaseWorker
    {
        private readonly AppSettings _appSettings;
        private readonly ILogger<FppVitalsWorker> _logger;
        private readonly ITwitterService _twitterService;
        private readonly IFppService _fppService;

        public FppVitalsWorker(ILogger<FppVitalsWorker> logger, AppSettings appSettings,
            IFppService fppService, ITwitterService twitterService)
             : base(logger, appSettings)
        {
            _appSettings = appSettings;
            _logger = logger;
            _twitterService = twitterService;
            _fppService = fppService;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting vitals worker");
            
            string previousSecondsPlayed = string.Empty;
            string previousSecondsRemaining = string.Empty;

            await _twitterService.GetAuthenticatedUserAsync();

            while (!stoppingToken.IsCancellationRequested)
            {
                _fppService.ResetAlarmCount();

                try
                {
                    FalconFppdMultiSyncSystemsDto syncStatus = await _fppService.GetMultiSyncStatusAsync(AppConstants.Localhost);

                    foreach (var fppInstance in syncStatus.RemoteSystems)
                    {
                        _logger.LogInformation($"Checking vitals for {fppInstance.Address}");

                        FalconFppdStatusDto falconFppdStatus = await _fppService.GetFppdStatusAsync(fppInstance.Address);

                        if (falconFppdStatus == null)
                        {
                            _logger.LogError(ExceptionMessage.FppOffline);
                            break;
                        }

                        // check sensors
                        foreach (var sensor in falconFppdStatus.Sensors)
                        {
                            if (sensor.ValueType.ToLower() == "temperature" && sensor.Value > _appSettings.Monitoring.MaxCpuTemperatureC)
                            {
                                string alarmMessage = $"Temperature warning! Temperature: {sensor.Value}; limit: {_appSettings.Monitoring.MaxCpuTemperatureC}";
                                await _twitterService.PostTweetAlarmAsync(alarmMessage);
                            }
                        }

                        // check for stuck songs
                        if (falconFppdStatus.Mode_Name == FppMode.Master || falconFppdStatus.Mode_Name == FppMode.Standalone)
                        {
                            if (previousSecondsPlayed == falconFppdStatus.Seconds_Played ||
                                previousSecondsRemaining == falconFppdStatus.Seconds_Remaining)
                            {
                                var task = Task.Run(() => _twitterService.PostTweetAlarmAsync(ExceptionMessage.FppFrozen));
                            }

                            previousSecondsPlayed = falconFppdStatus.Seconds_Played;
                            previousSecondsRemaining = falconFppdStatus.Seconds_Remaining;
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError(ex, ExceptionMessage.NoInternetConnection + ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }

                await Task.Delay(TimeSpan.FromSeconds(DelaySeconds.Medium), stoppingToken);
            }
        }
    }
}