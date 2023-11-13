using System.Text.Json.Serialization;

namespace Almostengr.Common.HomeAssistant.Common;

public abstract class BaseSwitchRequest
{
    public BaseSwitchRequest(string entityId)
    {
        if (string.IsNullOrWhiteSpace(entityId))
        {
            throw new ArgumentNullException(nameof(entityId));
        }

        EntityId = entityId;
    }

    [JsonPropertyName("entity_id")]
    public string EntityId { get; init; }
}