using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Almostengr.FalconPiMonitor.Models
{
    public class WeatherAlerts
    {
        public List<Features> Features { get; set; }
    }

    public class Features
    {
        [Required]
        [JsonPropertyName("Properties")]
        public AlertProperties Properties { get; set; }
    }

    public class AlertProperties
    {
        public string Id { get; set; }
        public string Event { get; set; }
        public DateTime Expires { get; set; }
        public DateTime Effective { get; set; }
        public string Urgency { get; set; }
        public string Status { get; set; }
        public string Headline { get; set; }
    }
}