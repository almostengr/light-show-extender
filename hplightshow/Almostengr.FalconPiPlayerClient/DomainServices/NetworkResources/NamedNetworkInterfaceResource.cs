using System.Text.Json.Serialization;
using Almostengr.FalconPiPlayerClient.DomainServices.Resources;

namespace Almostengr.FalconPiPlayerClient.DomainServices.NetworkResources;

public class CoreNetworkInterfaceResource : BaseResource
{
    [JsonPropertyName("INTERFACE")]
    public string Interface { get; set; }

    [JsonPropertyName("PROTO")]
    public string Protocol { get; set; }

    [JsonPropertyName("ADDRESS")]
    public string Address { get; set; }

    [JsonPropertyName("NETMASK")]
    public string NetMask { get; set; }

    [JsonPropertyName("GATEWAY")]
    public string Gateway { get; set; }
}

public sealed class NamedNetworkInterfaceResource : CoreNetworkInterfaceResource
{
    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("CurrentAddress")]
    public string CurrentAddress { get; set; }

    [JsonPropertyName("CurrentNetmask")]
    public string CurrentNetmask { get; set; }
}
