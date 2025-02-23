using System.Text.Json.Serialization;
using Almostengr.FalconPiPlayerClient.DomainServices.Resources;

namespace Almostengr.FalconPiPlayerClient.DomainServices.NetworkResources;

public sealed class NetworkInterfaceResource : BaseResource
{
    [JsonPropertyName("ifindex")]
    public int IfIndex {get;set;}
}