using System.Text.Json.Serialization;
using Almostengr.Common.DomainServices;
using Almostengr.HpLightShow.WebApi.Features.Monitoring.Domain;

namespace Almostengr.HpLightShow.WebApi.Features.Monitoring.DomainServices.Resources;

public sealed class FppdStatusResource : BaseResource
{
    public List<Sensor> Sensors { get; init; } = new();

    [JsonPropertyName("current_song")]
    public string Current_Song { get; init; } = string.Empty;

    [JsonPropertyName("current_sequence")]
    public string Current_Sequence { get; init; } = string.Empty;

    public FppStatusOption Status { get; init; } = 0;
    public List<string> Warnings { get; init; } = new();

    public sealed class Sensor
    {
        public string Label { get; init; } = string.Empty;
        public double Value { get; init; }
    }
}