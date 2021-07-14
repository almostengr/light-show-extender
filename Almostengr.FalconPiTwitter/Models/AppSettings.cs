namespace Almostengr.FalconPiTwitter.Models
{
    public class AppSettings
    {
        public Twitter Twitter { get; set; }
        public Alarm Alarm { get; set; }
        public bool MonitorOnly { get; set; } = false;
    }

    public class Alarm
    {
        public string TwitterAlarmUser { get; set; }
    }

    public class Twitter
    {
        public string ConsumerSecret { get; set; }
        public string ConsumerKey { get; set; }
        public string AccessToken { get; set; }
        public string AccessSecret { get; set; }
    }

}