using System.Text.Json.Serialization;
using Almostengr.Extensions;

namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public static class GetStatusHandler
{
    public static async Task<FppStatusResponse> Handle(IFppHttpClient fppHttpClient, CancellationToken cancellationToken, string hostname = "")
    {
        return await fppHttpClient.GetFppdStatusAsync(cancellationToken, hostname);
    }
}

public sealed class FppStatusResponse : BaseResponse
{
    public List<Sensor> Sensors { get; init; } = new();

    [JsonPropertyName("current_song")]
    public string Current_Song { get; init; } = string.Empty;

    [JsonPropertyName("current_sequence")]
    public string Current_Sequence { get; init; } = string.Empty;

    [JsonPropertyName("seconds_remaining")]
    public string Seconds_Remaining { get; init; } = string.Empty;

    [JsonPropertyName("scheduler")]
    public ScheduleDetail Scheduler { get; init; } = new();

    public sealed class ScheduleDetail
    {
        [JsonPropertyName("currentPlaylist")]
        public ActivePlaylist CurrentPlaylist { get; init; } = new();

        public sealed class ActivePlaylist
        {
            [JsonPropertyName("scheduledEndTime")]
            public string Playlist { get; init; } = string.Empty;
        }
    }

    public sealed class Sensor
    {
        public string Label { get; init; } = string.Empty;
        public double Value { get; init; }
    }
}
