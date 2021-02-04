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
                    FalconFppdStatus falconStatus = await GetCurrentStatusAsync();
                    FalconMediaMeta falconStatusMediaMeta = 
                        await GetCurrentSongMetaDataAsync(falconStatus.Current_Song);

                    if (string.IsNullOrEmpty(falconStatusMediaMeta.Format.Tags.Title))
                    {
                        falconStatusMediaMeta.Format.Tags.Title = falconStatus.Current_Song_NotFile;
                    }

                    previousSong = await PostCurrentSongAsync(
                        previousSong, falconStatusMediaMeta.Format.Tags.Title,
                        falconStatusMediaMeta.Format.Tags.Artist,
                        falconStatusMediaMeta.Format.Tags.Album,
                        falconStatus.Current_PlayList.Playlist);
                }
                catch (NullReferenceException ex)
                {
                    ExceptionLogger(ex, string.Concat("Null exception ", ex.Message));
                }
                catch (SocketException ex)
                {
                    ExceptionLogger(ex, string.Concat("Socket Exception ", ex.Message));
                }
                catch (HttpRequestException ex)
                {
                    ExceptionLogger(ex, string.Concat("Http Request Exception. Are you connected to internet? ", ex.Message));
                }
                catch (Exception ex)
                {
                    ExceptionLogger(ex, string.Concat("Generic Exception. ", ex.Message));
                }

                await Task.Delay(TimeSpan.FromSeconds(AppSettings.FppMonitor.RefreshInterval));
            }
        }

        private async Task<FalconMediaMeta> GetCurrentSongMetaDataAsync(string songFileName)
        {
            string url = string.Concat(AppSettings.FalconPiPlayer.FalconUri, "media/", songFileName, "/meta");
            string responseString = await GetRequestAsync(url);
            return JsonConvert.DeserializeObject<FalconMediaMeta>(responseString);
        }

        private async Task<string> PostCurrentSongAsync(string prevSongTitle, string currSongTitle,
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
                tweet += string.Concat("\"", currSongTitle, "\"");
            }
            else
            {
                logger.LogInformation("Not tweeting song as it does not have title.");
                return prevSongTitle;
            }

            tweet += string.IsNullOrEmpty(songArtist) ? "" : string.Concat(" by ", songArtist);
            tweet += string.IsNullOrEmpty(songAlbum) ? "" : string.Concat(" (", songAlbum, ")");
            tweet += string.Concat(" at ", DateTime.Now.ToLongTimeString());

            await PostTweetAsync(tweet);

            return currSongTitle;
        }

    } // end class
}