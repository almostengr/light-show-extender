using System.Text.Json.Serialization;
using Almostengr.LightShowExtender.DomainService.Common;

namespace Almostengr.LightShowExtender.DomainService.Wled;

public sealed class WledJsonResponseDto : BaseResponseDto
{
    [JsonPropertyName("state")]
    public WledState State { get; init; } = new();

    [JsonPropertyName("info")]
    public WledInfo Info { get; init; } = new();

    public sealed class WledState
    {
        [JsonPropertyName("on")]
        public bool On { get; init; } = false;
    }

    public sealed class WledInfo
    {
        [JsonPropertyName("name")]
        public string Name { get; init; } = string.Empty;
    }
}