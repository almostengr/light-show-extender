using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Almostengr.FalconPiMonitor.Models
{
    public class WeatherStation
    {
        [JsonPropertyName("Properties")]
        public StationProperties Properties { get; set; }
    }

    public class StationProperties
    {
        public string Forecast { get; set; }
        public string StationIdentifier { get; set; }
    }
}