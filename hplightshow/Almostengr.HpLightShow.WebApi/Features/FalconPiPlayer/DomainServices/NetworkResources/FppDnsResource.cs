using System.Text.Json.Serialization;
using Almostengr.Common.DomainServices;

namespace Almostengr.FalconPiPlayerClient.DomainServices.NetworkResources;

public class FppDnsResource : BaseResource
{
    [JsonPropertyName("DNS1")]
    public string Dns1 { get; set; }

    [JsonPropertyName("DNS2")]
    public string Dns2 { get; set; }
}

public class FppDnsOutputResource : StatusResponseResource
{
    [JsonPropertyName("DNS")]
    public FppDnsResource DNS { get; set; }
}