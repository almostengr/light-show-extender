using System.Text.Json.Serialization;
using Almostengr.LightShowExtender.DomainService.Common;

namespace Almostengr.LightShowExtender.DomainService.Wled;

public sealed class WledJsonStateRequestDto : BaseRequestDto
{
    public WledJsonStateRequestDto(bool onState, uint brightness = 255)
    {
        On = onState;
        Brightness = brightness;
    }

    public bool On { get; init; }

    [JsonPropertyName("bri")]
    public uint Brightness { get; init; }
}