using System.Text.Json.Serialization;
using Almostengr.Extensions;

namespace Almostengr.LightShowExtender.DomainService.Wled;

public sealed class WledJsonStateResponse : BaseResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; init; } = false;
}
