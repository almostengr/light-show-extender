using System.Text.Json.Serialization;
using Almostengr.HttpClient;

namespace Almostengr.Common.HomeAssistant;

public abstract class BaseSwitchRequest : BaseRequest
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