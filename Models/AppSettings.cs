using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Almostengr.FalconPiMonitor.Models
{
    public class AppSettings
    {
        [Required]
        public Twitter Twitter { get; set; }

        [Required]
        private string _falconPiPlayerUrl { get; set; }
        public string FalconPiPlayerUrl
        {
            get { return _falconPiPlayerUrl; }
            set { _falconPiPlayerUrl = SetFalconPiHostname(value); }
        }

        IList<string> FalconPiRemotes { get; set; }
        public Alarm Alarm { get; set; }

        private string SetFalconPiHostname(string uri)
        {
            if (uri.StartsWith("http://") == false && uri.StartsWith("https://") == false)
            {
                uri = string.Concat("http://", uri);
            }

            uri = uri.Contains("/api") ? uri : string.Concat(uri, "/api");
            uri = uri.Replace("//api/", "/api/");

            return uri;
        }
    }

    public class Alarm
    {
        public string TwitterAlarmUser { get; set; }
        private double _maxTemperature { get; set; }
        public double MaxTemperature
        {
            get { return _maxTemperature; }
            set { _maxTemperature = value > 0.0 ? value : 55.0; }
        }
    }

    public class Twitter
    {
        [Required]
        public string ConsumerSecret { get; set; }
        [Required]
        public string ConsumerKey { get; set; }
        [Required]
        public string AccessToken { get; set; }
        [Required]
        public string AccessSecret { get; set; }
        public List<string> AlarmUsers { get; set; }
    }

    // public class Weather
    // {
    //     [Required]
    //     public string StationId { get; set; }
    //     [Required]
    //     public string EmailAddress { get; set; }
    //     [Required]
    //     public string WebsiteUrl { get; set; }
    //     public List<string> AlertTypes { get; set; }
    //     public double MaxTemperature { get; set; }
    //     public double MinTemperature { get; set; }
    //     public bool AlertTriggerShutdown { get; set; }
    //     public double MaxWindSpeed { get; set; }
    // }
}