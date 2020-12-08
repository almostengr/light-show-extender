using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static Almostengr.FalconPiMonitor.Logger;

namespace Almostengr.FalconPiMonitor
{
    public class FppMonitor
    {
        private readonly FalconApi _falconApi = new FalconApi();
        private int timeDelay = 1;

        public async Task RunMonitor()
        {
            string previousSong = "";

            LogMessage("Monitoring show");

            while (true)
            {
                FalconStatus falconStatus = await _falconApi.GetCurrentStatus();
                FalconStatusMediaMeta falconStatusMediaMeta = await _falconApi.GetCurrentSongMetaData(falconStatus.Current_Song);

                try
                {
                    previousSong = await PostCurrentSong(previousSong,
                        falconStatusMediaMeta.Format.Tags.Title, falconStatusMediaMeta.Format.Tags.Artist,
                        falconStatusMediaMeta.Format.Tags.Album);
                    await TemperatureCheck(falconStatus.Sensors);
                }
                catch (NullReferenceException ex)
                {
                    LogMessage(string.Concat("Exception occurred: ", ex.Message));
                }

#if RELEASE
                Thread.Sleep(TimeSpan.FromMinutes(timeDelay));
#else
                Thread.Sleep(TimeSpan.FromSeconds(10));
#endif
            }
        }

        private async Task<string> PostCurrentSong(string prevSongTitle, string currSongTitle, string currSongArtist, string currSongAlbum)
        {
            if (prevSongTitle == currSongTitle)
            {
                return prevSongTitle;
            }

            // TODO tweet out the current song
            LogMessage("Updating current song");

            string tweet = string.Concat("Now playing \"", currSongTitle, "\" by ", currSongArtist,
                " (", currSongAlbum, ") at ", DateTime.Now.ToShortTimeString());
            LogMessage(tweet);

            return currSongTitle;
        }

        private void CheckSchedulerStatus()
        {

        }

        private void NextShowTime()
        {

        }

        private async Task TemperatureCheck(IList<FalconStatusSensor> sensors)
        {
            foreach (var sensor in sensors)
            {
                if (sensor.ValueType.ToLower() == "temperature")
                {
                    if (sensor.Value >= 55.0)
                    {
                        LogMessage(string.Concat("High temperature alert! ", sensor.Value));
                        timeDelay++;
                    }
                    break;
                }
            }
        }
    }
}