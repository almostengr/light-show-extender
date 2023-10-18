using System.Text.Json.Serialization;
using Almostengr.Common.Utilities;

namespace Almostengr.Common.HomeAssistant;

public abstract class BaseSwitchRequest : BaseRequestDto
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