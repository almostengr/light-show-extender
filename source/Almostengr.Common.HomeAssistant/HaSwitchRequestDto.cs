using System.Text.Json.Serialization;

namespace Almostengr.Common.HomeAssistant;

public sealed class HaSwitchRequestDto
{
    public HaSwitchRequestDto(string entityId)
    {
        EntityId = entityId;
    }

    [JsonPropertyName("entity_id")]
    public string EntityId { get; init; }
}
