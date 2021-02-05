using System;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiMonitor.Models;
using Almostengr.FalconPiMonitor.ServicesBase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Almostengr.FalconPiMonitor.Services
{
    public class FppCurrentSongService : FppBaseService
    {
        public FppCurrentSongService(ILogger<FppBaseService> logger, IConfiguration configuration) : base(logger, configuration)
        {
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string previousSong = "";
            string masterInstance = 
                AppSettings.FalconPiPlayers
                    .Find(p => p.FalconPiPlayerMode.ToLower() == "master" ||
                        p.FalconPiPlayerMode.ToLower() == "player")
                    .Hostname;

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    FalconFppdStatus falconStatus = await GetCurrentStatusAsync(masterInstance);
                    FalconMediaMeta falconStatusMediaMeta = 
                        await GetCurrentSongMetaDataAsync(masterInstance, falconStatus.Current_Song);

                    if (string.IsNullOrEmpty(falconStatusMediaMeta.Format.Tags.Title))
                    {
                        falconStatusMediaMeta.Format.Tags.Title = falconStatus.Current_Song_NotFile;
                    }

                    previousSong = await PostCurrentSongAsync(
                        previousSong, 
                        falconStatusMediaMeta.Format.Tags.Title,
                        falconStatusMediaMeta.Format.Tags.Artist,
                        falconStatusMediaMeta.Format.Tags.Album,
                        falconStatus.CurrentPlayList.Playlist);
                }
                catch (NullReferenceException ex)
                {
                    // ExceptionLogger(ex, string.Concat("Null exception ", ex.Message));
                    ExceptionLogger<NullReferenceException>(ex);
                }
                catch (SocketException ex)
                {
                    // ExceptionLogger(ex, string.Concat("Socket Exception ", ex.Message));
                    ExceptionLogger<SocketException>(ex);
                }
                catch (HttpRequestException ex)
                {
                    // ExceptionLogger(ex, string.Concat("Http Request Exception. Are you connected to internet? ", ex.Message));
                    ExceptionLogger<HttpRequestException>(ex, "Are you connected to internet?");
                }
                catch (Exception ex)
                {
                    // ExceptionLogger(ex, string.Concat("Generic Exception. ", ex.Message));
                    ExceptionLogger<Exception>(ex);
                }

                await Task.Delay(TimeSpan.FromSeconds(AppSettings.MonitorRefreshInterval));
            }
        }

        private async Task<FalconMediaMeta> GetCurrentSongMetaDataAsync(string masterFppUrl, string songFileName)
        {
            // string url = string.Concat(AppSettings.FalconPiPlayers.Find(f => f.FalconPiPlayerMode.ToLower() == "master").FalconUrl, "media/", songFileName, "/meta");
            string url = string.Concat(masterFppUrl, "/media/", songFileName, "/meta");
            return await GetRequestAsync<FalconMediaMeta>(url);
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
                logger.LogError("Not tweeting song as it does not have title.");
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