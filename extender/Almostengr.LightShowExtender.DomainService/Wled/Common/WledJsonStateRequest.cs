using System.Text.Json.Serialization;
using Almostengr.Extensions;

namespace Almostengr.LightShowExtender.DomainService.Wled;

public sealed class WledJsonStateRequest : IQueryRequest
{
    public WledJsonStateRequest(bool onState, uint brightness = 255)
    {
        On = onState;
        Brightness = brightness;
    }

    [JsonPropertyName("on")]
    public bool On { get; init; }

    [JsonPropertyName("bri")]
    public uint Brightness { get; init; }
}