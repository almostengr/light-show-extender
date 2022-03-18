using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.Common;
using Almostengr.FalconPiTwitter.Common.Constants;
using Almostengr.FalconPiTwitter.DataTransferObjects;
using Almostengr.FalconPiTwitter.Services;
using Microsoft.Extensions.Logging;

namespace Almostengr.FalconPiTwitter.Workers
{
    public class FppVitalsWorker : BaseWorker
    {
        private readonly ILogger<FppVitalsWorker> _logger;
        private readonly IFppService _fppService;

        public FppVitalsWorker(ILogger<FppVitalsWorker> logger, AppSettings appSettings,
            IFppService fppService) : base(logger, appSettings)
        {
            _logger = logger;
            _fppService = fppService;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string previousSecondsPlayed = string.Empty;
            string previousSecondsRemaining = string.Empty;

            while (!stoppingToken.IsCancellationRequested)
            {
                _fppService.ResetAlarmCount();

                FalconFppdMultiSyncSystemsDto syncStatus = null;

                try
                {
                    syncStatus = await _fppService.GetMultiSyncStatusAsync(AppConstants.Localhost);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }

                foreach (var fppInstance in syncStatus.RemoteSystems)
                {
                    try
                    {
                        _logger.LogInformation($"Checking vitals for {fppInstance.Address}");

                        FalconFppdStatusDto falconFppdStatus = await _fppService.GetFppdStatusAsync(fppInstance.Address);

                        if (falconFppdStatus == null)
                        {
                            _logger.LogError(ExceptionMessage.FppOffline);
                            break;
                        }

                        await _fppService.CheckCpuTemperatureAsync(falconFppdStatus);

                        await _fppService.CheckStuckSongAsync(falconFppdStatus, previousSecondsPlayed, previousSecondsRemaining);

                        if (falconFppdStatus.Mode_Name == FppMode.Master || falconFppdStatus.Mode_Name == FppMode.Standalone)
                        {
                            previousSecondsPlayed = falconFppdStatus.Seconds_Played;
                            previousSecondsRemaining = falconFppdStatus.Seconds_Remaining;
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
                }

                await Task.Delay(TimeSpan.FromSeconds(DelaySeconds.Medium), stoppingToken);
            }
        }
        
    }
}