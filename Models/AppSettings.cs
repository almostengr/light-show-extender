using System.ComponentModel.DataAnnotations;

namespace Almostengr.FalconPiMonitor.Models
{
    public class AppSettings
    {
        [Required]
        public TwitterSettings TwitterSettings { get; set; }
        public AlarmSettings AlarmSettings { get; set; }
        public FppMonitorSettings FppMonitorSettings { get; set; }
        public FalconPiPlayerSettings FalconPiPlayerSettings { get; set; }
    }

    public class TwitterSettings
    {
        [Required]
        public string ConsumerSecret { get; set; }
        [Required]
        public string ConsumerKey { get; set; }
        [Required]
        public string AccessToken { get; set; }
        [Required]
        public string AccessSecret { get; set; }

        private bool _testing;
        public bool Testing
        {
            get { return _testing; }
            set { _testing = SetTesting(value); }
        }

        private bool SetTesting(bool? value)
        {
            if (value == null)
            {
                return false;
            }

            return (bool)value;
        }
    }

    public class AlarmSettings
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

    public class FppMonitorSettings
    {
        public bool PostOffline { get; set; }

        private int _refreshInterval;
        public int RefreshInterval
        {
            get { return _refreshInterval; }
            set { _refreshInterval = SetRefreshInterval(value); }
        }

        private int SetRefreshInterval(int? interval)
        {
            if (interval == null || interval < 0)
            {
                interval = 15;
            }
            return (int)interval;
        }
    }

    public class FalconPiPlayerSettings
    {
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