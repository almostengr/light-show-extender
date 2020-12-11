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
        private FalconApi FalconApi {get; set;}
        private TwitterApi TwitterApi { get; set; }
        private bool TemperatureAlarm { get; set; }
        private double TemperatureThreshold { get; set; }
        private string AlarmAccount { get; set; }
        private bool PostOffline { get; set; }

        public FppMonitor(AppSettings settings)
        {
            AlarmAccount = settings.AlarmSettings.AlarmUser;
            TemperatureThreshold = settings.AlarmSettings.Temperature;
            PostOffline = settings.FppShow.PostOffline;
            TwitterApi = new TwitterApi(settings.TwitterSettings.ConsumerKey, settings.TwitterSettings.ConsumerSecret,
                settings.TwitterSettings.AccessToken, settings.TwitterSettings.AccessSecret);
            FalconApi = new FalconApi(settings.FalconPiSettings.FalconUri);
        }

        public async Task RunMonitor()
        {
            string previousSong = "";

            LogMessage("Monitoring show");
            LogMessage("Exit program by pressing Ctrl+C");

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
                    LogMessage(string.Concat("Null Exception occurred: ", ex.Message));
                }
                catch (SocketException ex)
                {
                    LogMessage(string.Concat("Socket Exception occurred: ", ex.Message));
                }
                catch (HttpRequestException ex)
                {
                    LogMessage(string.Concat("Are you connected to internet? HttpRequest Exception occured: ", ex.Message));
                }
                catch (Exception ex)
                {
                    LogMessage(string.Concat("Exception occurred: ", ex.Message));
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

            if (showOffline && PostOffline == false)
            {
                LogMessage("Show is offline. Not posting song");
                return currSongTitle;
            }

            DebugMessage("Updating current song");

            string tweet = "Playing ";

            if (string.IsNullOrEmpty(currSongTitle) == false)
            {
                tweet = string.Concat(tweet, "\"", currSongTitle, "\"");
            }
            else
            {
                LogMessage("Not tweeting song as it does not have title.");
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

            if (showOffline)
            {
                tweet = string.Concat(tweet, " [Offline]");
            }

            await TwitterApi.PostTweet(tweet);

            return currSongTitle;
        }

        // private void CheckSchedulerStatus()
        // {
        // }

        // private void NextShowTime(FalconStatusCurrentPlayList currentPlayList,
        //     FalconStatusNextPlaylist nextPlaylist)
        // {
        //     int currentHour = DateTime.Now.Hour;
        //     if (currentPlayList.Playlist == "Offline" && AlertedNextShowtime == false &&
        //         (currentHour == 13 || currentHour == 16))
        //     {
        //         string tweet = string.Concat("Next showtime is ", nextPlaylist.Start_Time);
        //         AlertedNextShowtime = true;
        //     }
        //     else if (currentHour != 13 && currentHour != 16)
        //     {
        //         AlertedNextShowtime = false;
        //     }
        // }

        private async Task TemperatureCheck(IList<FalconStatusSensor> sensors)
        {
            if (Double.IsNegative(TemperatureThreshold) || Double.IsNaN(TemperatureThreshold))
            {
                return;
            }

            foreach (var sensor in sensors)
            {
                if (sensor.ValueType.ToLower() == "temperature")
                {
                    if (sensor.Value >= TemperatureThreshold && TemperatureAlarm == false)
                    {
                        await TwitterApi.PostTweet(
                            string.Concat(AlarmAccount, " High temperature alert! ", sensor.Value, "C, ",
                                sensor.DegreesCToF(), "F"));
                        TemperatureAlarm = true;
                    }
                    else if (sensor.Value < TemperatureThreshold)
                    {
                        TemperatureAlarm = false;
                    }
                    break;
                }
            }
        }
    }
}