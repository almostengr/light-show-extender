using System.Text.Json.Serialization;
using Almostengr.Common.DomainServices;

namespace Almostengr.FalconPiPlayerClient.DomainServices.NetworkResources;

public class StatusOnlyResource : BaseResource
{
    [JsonPropertyName("status")]
    public string Status { get; set; }
}