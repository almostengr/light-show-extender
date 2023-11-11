using System.Text.Json.Serialization;
using Almostengr.Extensions;

namespace Almostengr.Common.HomeAssistant;

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