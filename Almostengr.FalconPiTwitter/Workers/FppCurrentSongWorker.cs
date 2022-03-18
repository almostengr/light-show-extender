using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.DataTransferObjects;
using Microsoft.Extensions.Logging;
using Almostengr.FalconPiTwitter.Common.Constants;
using Almostengr.FalconPiTwitter.Services;
using Almostengr.FalconPiTwitter.Common;

namespace Almostengr.FalconPiTwitter.Workers
{
    public class FppCurrentSongWorker : BaseWorker
    {
        private readonly ILogger<FppCurrentSongWorker> _logger;
        private readonly AppSettings _appSettings;
        private readonly ITwitterService _twitterService;
        private readonly IFppService _fppService;

        public FppCurrentSongWorker(ILogger<FppCurrentSongWorker> logger, AppSettings appSettings,
            IFppService fppService, ITwitterService twitterService)
         : base(logger, appSettings)
        {
            _logger = logger;
            _appSettings = appSettings;
            _fppService = fppService;
            _twitterService = twitterService;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string previousSong = string.Empty;

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(DelaySeconds.Short), stoppingToken);

                try
                {
                    FalconFppdStatusDto falconFppdStatus = 
                        await _fppService.GetFppdStatusAsync(_appSettings.FppHosts[0]);

                    if (falconFppdStatus.Mode_Name == FppMode.Remote)
                    {
                        _logger.LogDebug("This is remote instance of FPP. Exiting");
                        break;
                    }

                    if (falconFppdStatus.Current_Song == string.Empty)
                    {
                        _logger.LogDebug("No song is currently playling");
                        continue ;
                    }

                    FalconMediaMetaDto falconMediaMeta =
                        await _fppService.GetCurrentSongMetaDataAsync(falconFppdStatus.Current_Song);

                    _logger.LogDebug("Getting song title");
                    falconMediaMeta.Format.Tags.Title =
                        string.IsNullOrEmpty(falconMediaMeta.Format.Tags.Title) ?
                        falconFppdStatus.Current_Song_NotFile :
                        falconMediaMeta.Format.Tags.Title;

                    previousSong = await _twitterService.PostCurrentSongAsync(
                        previousSong, falconMediaMeta.Format.Tags.Title,
                        falconMediaMeta.Format.Tags.Artist,
                        falconFppdStatus.Current_PlayList.Playlist);
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError(ExceptionMessage.NoInternetConnection + ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
            }
        }

    }
}