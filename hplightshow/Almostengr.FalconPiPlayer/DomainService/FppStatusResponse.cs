using System.Text.Json.Serialization;
using Almostengr.Common.Query;

namespace Almostengr.FalconPiPlayer.DomainService;

public sealed class FppStatusResponse : IQueryResponse
{
    public List<Sensor> Sensors { get; init; } = new();

    [JsonPropertyName("current_song")]
    public string Current_Song { get; init; } = string.Empty;

    [JsonPropertyName("current_sequence")]
    public string Current_Sequence { get; init; } = string.Empty;

    public int Status { get; init; } = 0;

    public sealed class Sensor
    {
        public string Label { get; init; } = string.Empty;
        public double Value { get; init; }
    }
}
