using System.Text.Json.Serialization;
using Almostengr.Extensions;

namespace Almostengr.Wled.DomainService

public sealed class WledJsonStateResponse : IQueryResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; init; } = false;
}
