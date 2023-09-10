using System.Text.Json.Serialization;
using Almostengr.LightShowExtender.DomainService.Common;

namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public sealed class FppStatusDto : BaseDto
{
    public List<Sensor> Sensors { get; init; } = new();

    [JsonPropertyName("current_song")]
    public string Current_Song { get; init; } = string.Empty;

    [JsonPropertyName("seconds_remaining")]
    public string Seconds_Remaining { get; init; } = string.Empty;

    public string Status_Name { get; init; } = string.Empty;
    public ScheduleDetail Scheduler { get; init; } = new();
    public uint Volume { get; init; }

    public class ScheduleDetail
    {
        [JsonPropertyName("currentPlaylist")]
        public ActivePlaylist CurrentPlaylist { get; init; } = new();

        public class ActivePlaylist
        {
            [JsonPropertyName("scheduledEndTime")]
            public uint ScheduledEndTime { get; init; }
        }
    }

    public class Sensor
    {
        public string Label { get; init; } = string.Empty;
        public double Value { get; init; }
    }
}
