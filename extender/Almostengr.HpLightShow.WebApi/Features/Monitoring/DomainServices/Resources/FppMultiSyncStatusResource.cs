using System.Text.Json.Serialization;

namespace Almostengr.HpLightShow.WebApi.Features.Monitoring.DomainServices.Resources;

public sealed class FppMultiSyncStatusResource : StatusResponseResource
{
    [JsonPropertyName("masterHostname")]
    public string MasterHostname { get; set; }

    [JsonPropertyName("masterIP")]
    public string MasterIP { get; set; }

    [JsonPropertyName("systems")]
    public List<FppdSystem> Systems { get; set; } = new();

    public class FppdSystem
    {
        [JsonPropertyName("hostname")]
        public string Hostname { get; set; }

        [JsonPropertyName("lastRecieveTime")]
        public DateTime LastReceiveTime { get; set; }

        [JsonPropertyName("pktBlank")]
        public int PktBlank { get; set; }

        [JsonPropertyName("pktCommand")]
        public int PktCommand { get; set; }

        [JsonPropertyName("pktError")]
        public int PktError { get; set; }

        [JsonPropertyName("pktFPPCommand")]
        public int PktFppCommand { get; set; }

        [JsonPropertyName("pktPing")]
        public int PktPing { get; set; }

        [JsonPropertyName("pktPlugin")]
        public int PktPlugin { get; set; }

        [JsonPropertyName("pktSyncMedOpen")]
        public int PktSyncMedOpen { get; set; }

        [JsonPropertyName("pktSyncMedStart")]
        public int PktSyncMedStart { get; set; }

        [JsonPropertyName("pktSyncMedStop")]
        public int PktSyncMedStop { get; set; }

        [JsonPropertyName("pktSyncMedSync")]
        public int PktSyncMedSync { get; set; }

        [JsonPropertyName("pktSyncSeqStart")]
        public int PktSyncSeqStart { get; set; }

        [JsonPropertyName("pktSyncSeqStop")]
        public int PktSyncSeqStop { get; set; }

        [JsonPropertyName("pktSyncSeqSync")]
        public int PktSyncSeqSync { get; set; }

        [JsonPropertyName("sourceIP")]
        public string SourceIP { get; set; }
    }
}
