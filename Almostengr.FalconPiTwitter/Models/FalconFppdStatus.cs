using System.Collections.Generic;

namespace Almostengr.FalconPiTwitter.Models
{
    public class FalconFppdStatus : ModelBase
    {
        public FalconFppdStatusCurrentPlayList Current_PlayList { get; set; }
        public List<FalconFppdStatusSensor> Sensors { get; set; }
        public string Current_Song { get; set; }
        public FalconFppdStatusNextPlaylist Next_Playlist { get; set; }

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
    }

    public class FalconFppdStatusCurrentPlayList
    {
        public string Playlist { get; set; }
    }
}