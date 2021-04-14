using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Almostengr.FalconPiMonitor.Models
{
    public class AppSettings
    {
        [Required]
        public Twitter Twitter { get; set; }
        // public Alarm Alarm { get; set; }
        // public FppMonitor FppMonitor { get; set; }

        [Required]
        private int _monitorRefreshInterval;
        public int MonitorRefreshInterval
        {
            get { return _monitorRefreshInterval; }
            set { _monitorRefreshInterval = SetRefreshInterval(value); }
        }
        public List<FalconPiPlayer> FalconPiPlayers { get; set; }
        public Weather Weather { get; set; }

        private int SetRefreshInterval(int? interval)
        {
            if (interval == null || interval < 5)
            {
                interval = 10;
            }
            return (int)interval;
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
        [Required]
        public List<string> AlarmUsers { get; set; }
    }

    public class Weather
    {
        [Required]
        public string StationId { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string WebsiteUrl { get; set; }
        public List<string> AlertTypes { get; set; }
        public double MaxTemperature { get; set; }
        public double MinTemperature { get; set; }
        public bool AlertTriggerShutdown { get; set; }
        public double MaxWindSpeed { get; set; }
    }

    // public class Alarm
    // {
    // [Required]
    // private double _tempThreshold;
    // public double TempThreshold
    // {
    //     get { return _tempThreshold; }
    //     set { _tempThreshold = SetTempThreshold(value); }
    // }
    // // public string TwitterUser { get; set; }

    // private double SetTempThreshold(double? threshold)
    // {
    //     if (threshold == null || threshold < 0)
    //     {
    //         threshold = 55.0;
    //     }
    //     return (double)threshold;
    // }
    // }

    // public class FppMonitor
    // {
    // }

    public class FalconPiPlayer
    {
        [Required]
        private string _hostname;
        [Required]
        public string Hostname
        {
            get { return _hostname; }
            set { _hostname = SetFalconPiHostname(value); }
        }
        [Required]
        private double _maxCpuTemperature;
        public double MaxCpuTemperature
        {
            get { return _maxCpuTemperature; }
            set { _maxCpuTemperature = SetMaxCpuTemperature(value); }
        }
        // public FalconPiPlayerMode FalconPiPlayerMode { get; set; }
        public string FalconPiPlayerMode { get; set; }

        private string SetFalconPiHostname(string uri)
        {
            uri = uri.ToLower().Replace("api/", "").Replace("api", "");

            if (uri.StartsWith("http://") == false && uri.StartsWith("https://") == false)
            {
                uri = string.Concat("http://", uri);
            }

            uri = string.Concat(uri, "/api/");
            uri = uri.Replace("//api/", "/api/");

            return uri;
        }

        private double SetMaxCpuTemperature(double? threshold)
        {
            if (threshold == null || threshold < 0)
            {
                threshold = 55.0;
            }
            return (double)threshold;
        }
    }
}