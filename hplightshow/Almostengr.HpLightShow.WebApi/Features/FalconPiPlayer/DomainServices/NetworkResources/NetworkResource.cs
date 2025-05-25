using System.Text.Json.Serialization;
using Almostengr.Common.DomainServices;

namespace Almostengr.FalconPiPlayerClient.DomainServices.NetworkResources;

public sealed class NetworkResource : BaseResource
{
    [JsonPropertyName("status")]
    public string Status { get; set; }

    public List<Network> Networks { get; set; } = new();

    public class Network
    {
        [JsonPropertyName("lastSeen")]
        public string LastSeen { get; set; }

        [JsonPropertyName("freq")]
        public string Frequency { get; set; }

        [JsonPropertyName("signal")]
        public string Signal { get; set; }

        [JsonPropertyName("SSID")]
        public string SSID { get; set; }
    }
}