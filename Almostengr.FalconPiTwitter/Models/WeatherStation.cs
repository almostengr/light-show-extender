using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Almostengr.FalconPiTwitter.Models
{
    public class WeatherStation : ModelBase
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