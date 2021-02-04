using System.Text.Json.Serialization;

namespace Almostengr.FalconPiMonitor.Models
{
    public class WeatherZone
    {
        [JsonPropertyName("Properties")]
        public ZoneProperties Properties { get; set; }
    }

    public class ZoneProperties
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
    }
}