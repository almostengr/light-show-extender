using System.Text.Json.Serialization;
using Almostengr.LightShowExtender.DomainService.Common;

namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public sealed class FppStatusDto : BaseDto
{
    [JsonPropertyName("current_playlist")]
    public CurrentPlaylist Current_Playlist { get; init; } = new();
    public List<Sensor> Sensors { get; init; } = new();
    
    [JsonPropertyName("current_song")]
    public string Current_Song { get; init; } = string.Empty;

    [JsonPropertyName("seconds_remaining")]
    public string Seconds_Remaining { get; init; } = string.Empty;
    
    public string Status_Name { get; init; } = string.Empty;

    public class Sensor
    {
        public string Label { get; init; } = string.Empty;
        public double Value { get; init; }
    }

    public class CurrentPlaylist
    {
        public string Playlist { get; init; } = string.Empty;
    }
}
