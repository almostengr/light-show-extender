namespace Almostengr.FalconPiMonitor
{
    public class AppSettings
    {
        public TwitterConfig TwitterConfig { get; set; }
    }

    public class TwitterConfig
    {
        public string ConsumerSecret { get; set; }
        public string ConsumerKey { get; set; }
        public string AccessToken { get; set; }
        public string AccessSecret { get; set; }
        public string AlarmUser { get; set; }
    }

    public class AlarmSettings
    {
        public double Temperature { get; set; }
        public string AlarmUser { get; set; }
    }
}