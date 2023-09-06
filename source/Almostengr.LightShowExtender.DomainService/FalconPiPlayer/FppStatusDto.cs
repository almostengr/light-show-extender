using Almostengr.LightShowExtender.DomainService.Common;

namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public sealed class FppStatusDto : BaseDto
{
    public FalconFppdStatusCurrentPlayList Current_PlayList { get; init; } = new();
    public List<FalconFppdStatusSensor> Sensors { get; init; } = new();
    public string Current_Song { get; init; } = string.Empty;
    public FalconFppdStatusNextPlaylist Next_Playlist { get; init; } = new();
    public string Mode_Name { get; init; } = string.Empty;
    public string Fppd { get; init; } = string.Empty;
    public string Seconds_Played { get; init; } = string.Empty;
    public string Seconds_Remaining { get; init; } = string.Empty;
    public string Status_Name { get; init; } = string.Empty;

    public class FalconFppdStatusNextPlaylist
    {
        public string Playlist { get; init; } = string.Empty;
        public string Start_Time { get; init; } = string.Empty;
    }

    public class FalconFppdStatusSensor
    {
        public string Label { get; init; } = string.Empty;
        public double Value { get; init; }
        public string ValueType { get; init; } = string.Empty;
    }

    public class FalconFppdStatusCurrentPlayList
    {
        public string Playlist { get; init; } = string.Empty;
    }
}