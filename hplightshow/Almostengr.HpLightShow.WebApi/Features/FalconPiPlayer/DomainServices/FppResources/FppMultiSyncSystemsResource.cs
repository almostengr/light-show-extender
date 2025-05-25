using System.Text.Json.Serialization;

namespace Almostengr.FalconPiPlayerClient.DomainServices.Resources;


public sealed class FppMultiSyncSystemsResource : StatusResponseResource
{
    [JsonPropertyName("systems")]
    public List<FppSystem> Systems { get; set; } = new();

    public class FppSystem
    {
        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("channelRanges")]
        public string ChannelRanges { get; set; }

        [JsonPropertyName("fppMode")]
        public int FppMode { get; set; }

        [JsonPropertyName("fppModeString")]
        public string FppModeString { get; set; }

        [JsonPropertyName("hostname")]
        public string Hostname { get; set; }

        [JsonPropertyName("lastSeen")]
        public int LastSeen { get; set; }

        [JsonPropertyName("lastSeenStr")]
        public string LastSeenString { get; set; }

        [JsonPropertyName("majorVersion")]
        public int MajorVersion { get; set; }

        [JsonPropertyName("minorVersion")]
        public int MinorVersion { get; set; }

        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }
    }
}
