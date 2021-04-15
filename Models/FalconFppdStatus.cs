using System.Collections.Generic;

namespace Almostengr.FalconPiMonitor.Models
{
    public class FalconFppdStatus
    {
        public FalconFppdStatusCurrentPlayList Current_PlayList { get; set; }
        public List<FalconFppdStatusSensor> Sensors { get; set; }
        public string Current_Song { get; set; }
        public FalconFppdStatusNextPlaylist FalconFppdStatusNextPlaylist { get; set; }
        public string Status_Name { get; set; }

        public string Current_Song_NotFile
        {
            get { return GetCurrentSongNotFile(); }
        }

        private string GetCurrentSongNotFile()
        {
            return Current_Song.Replace(".mp3", "").Replace(".m4a", "").Replace(".ogg", "")
                    .Replace("_", " ").Replace("-", " ");
        }
    }

    public class FalconFppdStatusNextPlaylist
    {
        public string Playlist { get; set; }
        public string Start_Time { get; set; }
    }

    public class FalconFppdStatusSensor
    {
        public double Value { get; set; }
        public string ValueType { get; set; }

        public double DegreesCToF()
        {
            return (Value * 1.8) + 32;
        }
    }

    public class FalconFppdStatusCurrentPlayList
    {
        public string Playlist { get; set; }
    }
}