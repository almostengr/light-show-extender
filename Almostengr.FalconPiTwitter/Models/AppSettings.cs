namespace Almostengr.FalconPiTwitter.Models
{
    public class AppSettings
    {
        public Twitter Twitter { get; set; }
        public Alarm Alarm { get; set; }
        public bool MonitorOnly { get; set; } = false;

        // private string SetFalconPiHostname(string uri)
        // {
        //     if (uri.StartsWith("http://") == false && uri.StartsWith("https://") == false)
        //     {
        //         uri = string.Concat("http://", uri);
        //     }

        //     uri = uri.Replace("/api/", "/");

        //     return uri;
        // }
    }

    public class Alarm
    {
        public string TwitterAlarmUser { get; set; }

        private double _maxTemperature { get; set; }
        public double MaxTemperature
        {
            get { return _maxTemperature; }
            set { _maxTemperature = value > 0.0 ? value : 60.0; }
        }

        private int _maxAlarms;
        public int MaxAlarms
        {
            get { return _maxAlarms; }
            set { _maxAlarms = value > 0 ? value : 3; }
        }
    }

    public class Twitter
    {
        public string ConsumerSecret { get; set; }
        public string ConsumerKey { get; set; }
        public string AccessToken { get; set; }
        public string AccessSecret { get; set; }
    }

}