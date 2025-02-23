using System.Text.Json.Serialization;

namespace Almostengr.FalconPiPlayerClient.DomainServices.Resources;

public sealed class FppdE131StatsResource : StatusResource
{
    public List<Universe> Universes { get; set; } = new();

    public sealed class Universe
    {
        [JsonPropertyName("bytesReceived")]
        public int BytesReceived { get; set; }
        
        [JsonPropertyName("errors")]
        public int Errors { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("packetsReceived")]
        public string PacketsReceived { get; set; }

        [JsonPropertyName("startChannel")]
        public int StartChannel { get; set; }
    }
}
