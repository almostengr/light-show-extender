using System;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiMonitor.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Almostengr.FalconPiMonitor.Services
{
    public class FppCurrentSongService : BaseService
    {
        public FppCurrentSongService(ILogger<BaseService> logger, IConfiguration configuration) : base(logger, configuration)
        {
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string previousSong = "";

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    FalconFppdStatus falconStatus = await GetCurrentStatus();
                    FalconMediaMeta falconStatusMediaMeta = await GetCurrentSongMetaData(falconStatus.Current_Song);

                    if (falconStatusMediaMeta.Format.Tags.Title == "" || falconStatusMediaMeta.Format.Tags.Title == null)
                    {
                        falconStatusMediaMeta.Format.Tags.Title = falconStatus.Current_Song_NotFile;
                    }

                    previousSong = await PostCurrentSong(
                        previousSong, falconStatusMediaMeta.Format.Tags.Title,
                        falconStatusMediaMeta.Format.Tags.Artist,
                        falconStatusMediaMeta.Format.Tags.Album,
                        falconStatus.Current_PlayList.Playlist);
                }
                catch (NullReferenceException ex)
                {
                    logger.LogError(string.Concat("Null Exception. ", ex.Message));
                    logger.LogDebug(ex, ex.Message);
                }
                catch (SocketException ex)
                {
                    logger.LogError(string.Concat("Socket Exception. ", ex.Message));
                    logger.LogDebug(ex, ex.Message);
                }
                catch (HttpRequestException ex)
                {
                    logger.LogError(string.Concat("Http Request Exception. Are you connected to internet? ", ex.Message));
                    logger.LogDebug(ex, ex.Message);
                }
                catch (Exception ex)
                {
                    logger.LogError(string.Concat("Generic Exception. ", ex.Message));
                    logger.LogDebug(ex, ex.Message);
                }

                await Task.Delay(TimeSpan.FromSeconds(AppSettings.FppMonitor.RefreshInterval));
            }
        }

        private async Task<FalconMediaMeta> GetCurrentSongMetaData(string songFileName)
        {
            HttpResponseMessage response = await HttpClient.GetAsync(
                string.Concat(AppSettings.FalconPiPlayer.FalconUri, "media/", songFileName, "/meta"));

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<FalconMediaMeta>(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                throw new System.Exception(response.ReasonPhrase);
            }
        }

        private async Task<string> PostCurrentSong(string prevSongTitle, string currSongTitle,
            string songArtist = null, string songAlbum = null, string playlistName = "")
        {
            bool showOffline = IsTestingOrOfflinePlaylist(playlistName);

            if (prevSongTitle == currSongTitle)
            {
                return prevSongTitle;
            }

            if (showOffline)
            {
                logger.LogInformation($"Show is offline. Not posting song \"{currSongTitle}\"");
                return currSongTitle;
            }

            string tweet = "Playing ";

            if (string.IsNullOrEmpty(currSongTitle) == false)
            {
                tweet = string.Concat(tweet, "\"", currSongTitle, "\"");
            }
            else
            {
                logger.LogInformation("Not tweeting song as it does not have title.");
                return prevSongTitle;
            }

            if (string.IsNullOrEmpty(songArtist) == false)
            {
                tweet = string.Concat(tweet, " by ", songArtist);
            }

            if (string.IsNullOrEmpty(songAlbum) == false)
            {
                tweet = string.Concat(tweet, " (", songAlbum, ")");
            }

            tweet = string.Concat(tweet, " at ", DateTime.Now.ToLongTimeString());

            await PostTweet(tweet);

            return currSongTitle;
        }

    } // end class
}