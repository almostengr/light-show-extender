using System.Collections.Generic;

namespace Almostengr.FalconPiTwitter.DataTransferObjects
{
    public class FalconFppdStatusDto : BaseDto
    {
        public FalconFppdStatusCurrentPlayList Current_PlayList { get; set; }
        public List<FalconFppdStatusSensor> Sensors { get; set; }
        public string Current_Song { get; set; }
        public FalconFppdStatusNextPlaylist Next_Playlist { get; set; }
        public string Mode_Name { get; set; }
        public string Fppd { get; set; }
        public string Seconds_Played { get; set; }
        public string Seconds_Remaining { get; set; }
        public string Status_Name { get; set; }

        public string Current_Song_NotFile
        {
            get
            {
                return Current_Song.Replace(".mp3", "").Replace(".m4a", "").Replace(".ogg", "")
                  .Replace(".mp4", "").Replace("_", " ").Replace("-", " ");
            }
        }
    }

    public class FalconFppdStatusNextPlaylist
    {
        public string Playlist { get; set; }
        public string Start_Time { get; set; }
    }

    public class FalconFppdStatusSensor
    {
        public string Label { get; set; }
        public double Value { get; set; }
        public string ValueType { get; set; }
    }

    public class FalconFppdStatusCurrentPlayList
    {
        public string Playlist { get; set; }
    }
}