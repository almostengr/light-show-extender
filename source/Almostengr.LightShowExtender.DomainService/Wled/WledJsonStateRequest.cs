using System.Text.Json.Serialization;
using Almostengr.Common.Utilities;

namespace Almostengr.LightShowExtender.DomainService.Wled;

public sealed class WledJsonStateRequest : BaseRequest
{
    public WledJsonStateRequest(bool onState, uint brightness = 255)
    {
        On = onState;
        Brightness = brightness;
    }

    public bool On { get; init; }

    [JsonPropertyName("bri")]
    public uint Brightness { get; init; }
}