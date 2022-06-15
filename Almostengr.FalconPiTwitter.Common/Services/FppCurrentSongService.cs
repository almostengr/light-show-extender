using Almostengr.FalconPiTwitter.Clients;
using Almostengr.FalconPiTwitter.Common.Constants;
using Almostengr.FalconPiTwitter.Common.Extensions;
using Almostengr.FalconPiTwitter.DataTransferObjects;
using Microsoft.Extensions.Logging;
using Tweetinvi.Exceptions;

namespace Almostengr.FalconPiTwitter.Common.Services
{
    public class FppCurrentSongService : IFppCurrentSongService
    {
        private readonly IFppClient _fppClient;
        private readonly ILogger<FppCurrentSongService> _logger;
        private readonly AppSettings _appSettings;
        private readonly ITwitterService _twitterService;

        public FppCurrentSongService(IFppClient fppClient, ILogger<FppCurrentSongService> logger,
            ITwitterService twitterService, AppSettings appSettings)
        {
            _fppClient = fppClient;
            _logger = logger;
            _appSettings = appSettings;
            _twitterService = twitterService;
        }

        public async Task ExecuteCurrentSongWorkerAsync(CancellationToken stoppingToken)
        {
            string previousSong = string.Empty;

            while (stoppingToken.IsCancellationRequested == false)
            {
                await Task.Delay(TimeSpan.FromSeconds(DelaySeconds.Short), stoppingToken);

                try
                {
                    FalconFppdStatusDto fppStatus = await _fppClient.GetFppdStatusAsync(_appSettings.FppHosts[0]);

                    if (fppStatus.Mode_Name.IsRemoteInstance())
                    {
                        _logger.LogWarning("This is remote instance of FPP. Exiting");
                        break;
                    }

                    if (fppStatus.Current_Song.IsNullOrEmpty() || previousSong == fppStatus.Current_Song)
                    {
                        continue;
                    }

                    FalconMediaMetaDto falconMediaMeta = await _fppClient.GetCurrentSongMetaDataAsync(fppStatus.Current_Song);

                    if (falconMediaMeta.IsNull())
                    {
                        continue;
                    }

                    string songTitle = string.IsNullOrEmpty(falconMediaMeta.Format.Tags.Title) ?
                        fppStatus.Current_Song.GetSongNameFromFileName() :
                        falconMediaMeta.Format.Tags.Title;

                    await _twitterService.PostCurrentSongAsync(songTitle, falconMediaMeta.Format.Tags.Artist);

                    previousSong = fppStatus.Current_Song;
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError(ex.InnerException.ToString(), ExceptionMessage.NoInternetConnection + ex.Message);
                }
                catch (TwitterException ex)
                {
                    _logger.LogError(ex.InnerException.ToString(), ex.Message);
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.InnerException.ToString(), ex.Message);
                }
            }
        }

    }
}