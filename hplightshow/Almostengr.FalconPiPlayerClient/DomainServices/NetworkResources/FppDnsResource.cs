using System.Text.Json.Serialization;
using Almostengr.FalconPiPlayerClient.DomainServices.Resources;

namespace Almostengr.FalconPiPlayerClient.DomainServices.NetworkResources;

public class FppDnsResource : BaseResource
{
    [JsonPropertyName("DNS1")]
    public string Dns1 { get; set; }

    [JsonPropertyName("DNS2")]
    public string Dns2 { get; set; }
}

public class FppDnsOutputResource : StatusResource
{
    [JsonPropertyName("DNS")]
    public FppDnsResource DNS { get; set; }
}