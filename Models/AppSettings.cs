using System.ComponentModel.DataAnnotations;

namespace Almostengr.FalconPiMonitor.Models
{
    public class AppSettings
    {
        [Required]
        public Twitter Twitter { get; set; }
        public Alarm Alarm { get; set; }
        public FppMonitor FppMonitor { get; set; }
        public FalconPiPlayer FalconPiPlayer { get; set; }
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
        public bool TestModeEnabled { get; set; }
    }

    public class Alarm
    {
        [Required]
        private double _tempThreshold;
        public double TempThreshold
        {
            get { return _tempThreshold; }
            set { _tempThreshold = SetTempThreshold(value); }
        }
        public string TwitterUser { get; set; }

        private double SetTempThreshold(double? threshold)
        {
            if (threshold == null || threshold < 0)
            {
                threshold = 55.0;
            }
            return (double)threshold;
        }
    }

    public class FppMonitor
    {
        public bool PostOffline { get; set; }

        [Required]
        private int _refreshInterval;
        public int RefreshInterval
        {
            get { return _refreshInterval; }
            set { _refreshInterval = SetRefreshInterval(value); }
        }

        private int SetRefreshInterval(int? interval)
        {
            if (interval == null || interval < 5)
            {
                interval = 10;
            }
            return (int)interval;
        }
    }

    public class FalconPiPlayer
    {
        [Required]
        private string _falconPiUri;
        [Required]
        public string FalconUri
        {
            get { return _falconPiUri; }
            set { _falconPiUri = SetFalconPiUri(value); }
        }

        private string SetFalconPiUri(string uri)
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
    }
}