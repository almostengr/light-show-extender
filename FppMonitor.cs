using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using static Almostengr.FalconPiMonitor.Logger;

namespace Almostengr.FalconPiMonitor
{
    public class FppMonitor
    {
        private readonly FalconApi FalconApi = new FalconApi();
        private bool AlertedNextShowtime = false;
        private readonly TwitterApi TwitterApi = new TwitterApi();
        private bool TemperatureAlarm { get; set; }

        public async Task RunMonitor()
        {
            string previousSong = "";

            LogMessage("Monitoring show");

            while (true)
            {
                try
                {
                    if (previousSong == "")
                    {
                        await TwitterApi.GetLoggedInUser();
                    }

                    FalconStatus falconStatus = await FalconApi.GetCurrentStatus();
                    FalconStatusMediaMeta falconStatusMediaMeta = await FalconApi.GetCurrentSongMetaData(falconStatus.Current_Song);

                    if (falconStatusMediaMeta.Format.Tags.Title == "" || falconStatusMediaMeta.Format.Tags.Title == null)
                    {
                        falconStatusMediaMeta.Format.Tags.Title = falconStatus.Current_Song_NotFile;
                    }

                    previousSong = await PostCurrentSong(
                        previousSong, falconStatusMediaMeta.Format.Tags.Title,
                        falconStatusMediaMeta.Format.Tags.Artist, falconStatusMediaMeta.Format.Tags.Album,
                        falconStatus.Current_PlayList.Playlist.ToLower().Contains("offline"));

                    await TemperatureCheck(falconStatus.Sensors);
                }
                catch (NullReferenceException ex)
                {
                    LogMessage(string.Concat("Exception occurred: ", ex.Message));
                }
                catch (SocketException ex)
                {
                    LogMessage(string.Concat("Socket Exception occurred: ", ex.Message));
                }
                catch (HttpRequestException ex)
                {
                    LogMessage(ex.Message);
                }
                catch (Exception ex)
                {
                    LogMessage(string.Concat("General exception occurred: ", ex.Message));
                }


#if RELEASE
                Thread.Sleep(TimeSpan.FromSeconds(15));
#else
                Thread.Sleep(TimeSpan.FromSeconds(10));
#endif
            }
        }

        private async Task<string> PostCurrentSong(string prevSongTitle, string currSongTitle,
            string songArtist = null, string songAlbum = null, bool showOffline = false)
        {
            if (prevSongTitle == currSongTitle)
            {
                return prevSongTitle;
            }

            DebugMessage("Updating current song");

            string tweet = "Playing ";

            if (currSongTitle != null && currSongTitle != "Unknown" && currSongTitle != "")
            {
                tweet = string.Concat(tweet, "\"", currSongTitle, "\"");
            }
            else
            {
                LogMessage("Not posting song as it does not have title.");
                return prevSongTitle;
            }

            if (songArtist != null && songArtist != "")
            {
                tweet = string.Concat(tweet, " by ", songArtist);
            }

            if (songAlbum != null && songAlbum != "")
            {
                tweet = string.Concat(tweet, " (", songAlbum, ")");
            }

            tweet = string.Concat(tweet, " at ", DateTime.Now.ToLongTimeString());

            if (showOffline)
            {
                tweet = string.Concat(tweet, " [Offline]");
            }

            await TwitterApi.PostTweet(tweet);

            return currSongTitle;
        }

        private void CheckSchedulerStatus()
        {

        }

        private void NextShowTime(FalconStatusCurrentPlayList currentPlayList,
            FalconStatusNextPlaylist nextPlaylist)
        {
            int currentHour = DateTime.Now.Hour;

            if (currentPlayList.Playlist == "Offline" && AlertedNextShowtime == false &&
                (currentHour == 13 || currentHour == 16))
            {
                string tweet = string.Concat("Next showtime is ", nextPlaylist.Start_Time);
                AlertedNextShowtime = true;
            }
            else if (currentHour != 13 && currentHour != 16)
            {
                AlertedNextShowtime = false;
            }
        }

        private async Task TemperatureCheck(IList<FalconStatusSensor> sensors)
        {
            double tempThreshold = 55.0;

            foreach (var sensor in sensors)
            {
                if (sensor.ValueType.ToLower() == "temperature")
                {
                    if (sensor.Value >= tempThreshold && TemperatureAlarm == false)
                    {
                        await TwitterApi.PostTweet(
                            string.Concat("@almostengr High temperature alert! ", sensor.Value, "C, ",
                                sensor.DegreesCToF(), "F"));
                        TemperatureAlarm = true;
                    }
                    else if (sensor.Value < tempThreshold)
                    {
                        TemperatureAlarm = false;
                    }
                    break;
                }
            }
        }
    }
}