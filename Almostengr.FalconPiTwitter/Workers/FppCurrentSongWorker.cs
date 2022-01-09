using System;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.DataTransferObjects;
using Almostengr.FalconPiTwitter.Settings;
using Microsoft.Extensions.Logging;
using Almostengr.FalconPiTwitter.Constants;
using Almostengr.FalconPiTwitter.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Almostengr.FalconPiTwitter.Workers
{
    public class FppCurrentSongWorker : BaseWorker
    {
        private readonly ILogger<FppCurrentSongWorker> _logger;
        private readonly AppSettings _appSettings;
        private readonly ITwitterService _twitterService;
        private readonly IFppService _fppService;

        public FppCurrentSongWorker(ILogger<FppCurrentSongWorker> logger, AppSettings appSettings, IServiceScopeFactory factory)
         : base(logger)
        {
            _logger = logger;
            _appSettings = appSettings;
            _fppService = factory.CreateScope().ServiceProvider.GetRequiredService<IFppService>();
            _twitterService = factory.CreateScope().ServiceProvider.GetRequiredService<ITwitterService>();
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting current song worker");

            string previousSong = string.Empty;

            while (!stoppingToken.IsCancellationRequested)
            {
                FalconFppdStatusDto falconFppdStatus = await _fppService.GetFppdStatusAsync(_appSettings.FppHosts[0]);

                if (falconFppdStatus.Mode_Name == FppMode.Remote)
                {
                    _logger.LogDebug("This is remote instance of FPP");
                    await TaskDelayAsync(DelaySeconds.Smedium, stoppingToken);
                    continue;
                }

                if (falconFppdStatus.Current_Song == string.Empty)
                {
                    _logger.LogDebug("No song is currently playing");
                    await TaskDelayAsync(DelaySeconds.Smedium, stoppingToken);
                    continue;
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

                await Task.Delay(TimeSpan.FromSeconds(DelaySeconds.Short), stoppingToken);
            }
        }

    }
}