using System.Text.Json.Serialization;
using Almostengr.Common.Query;

namespace Almostengr.HomeAssistant.DomainService;

public abstract class BaseSwitchRequest : IQueryRequest
{
    public BaseSwitchRequest(string entityId)
    {
        EntityId = entityId;
    }

    [JsonPropertyName("entity_id")]
    public string EntityId { get; init; }
}