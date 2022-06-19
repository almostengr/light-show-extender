namespace Almostengr.FalconPiTwitter.Common.DataTransferObjects
{
    public class FalconFppdStatusDto : BaseDto
    {
        public FalconFppdStatusCurrentPlayList Current_PlayList { get; init; }
        public List<FalconFppdStatusSensor> Sensors { get; init; }
        public string Current_Song { get; init; }
        public FalconFppdStatusNextPlaylist Next_Playlist { get; init; }
        public string Mode_Name { get; init; }
        public string Fppd { get; init; }
        public string Seconds_Played { get; init; }
        public string Seconds_Remaining { get; init; }
        public string Status_Name { get; init; }
    }

    public class FalconFppdStatusNextPlaylist
    {
        public string Playlist { get; init; }
        public string Start_Time { get; init; }
    }

    public class FalconFppdStatusSensor
    {
        public string Label { get; init; }
        public double Value { get; init; }
        public string ValueType { get; init; }
    }

    public class FalconFppdStatusCurrentPlayList
    {
        public string Playlist { get; init; }
    }
}