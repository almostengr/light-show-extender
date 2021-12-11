using System.Collections.Generic;

namespace Almostengr.FalconPiTwitter.Settings
{
    public class AppSettings
    {
        public Twitter Twitter { get; set; }
        public Monitor Monitor { get; set; }
    }

    public class Twitter
    {
        public string ConsumerSecret { get; set; }
        public string ConsumerKey { get; set; }
        public string AccessToken { get; set; }
        public string AccessSecret { get; set; }
    }

    public class Monitor
    {
        public List<string> AlarmUsernames { get; set; } = new();
        public int MaxAllowedAlarms { get; set; } = 3;
        public double MaxCpuTemperatureC { get; set; } = 60.0;
        public List<string> HostUrls { get; set; } = new();
    }
}