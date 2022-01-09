using System.Collections.Generic;

namespace Almostengr.FalconPiTwitter.Settings
{
    public class AppSettings
    {
        public Twitter Twitter { get; set; }
        public Monitoring Monitoring { get; set; }
        public bool CountdownEnabled { get; set; } = false;
        public int MaxHashTags { get; set; } = 3;
        public List<string> FppHosts { get; set; } = null;
        public bool DemoMode { get; set; } = false;
    }

    public class Twitter
    {
        public string ConsumerSecret { get; set; } = string.Empty;
        public string ConsumerKey { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public string AccessSecret { get; set; } = string.Empty;
    }

    public class Monitoring
    {
        public List<string> AlarmUsernames { get; set; } = new();
        public int MaxAlarmsPerHour { get; set; } = 3;
        public double MaxCpuTemperatureC { get; set; } = 60.0;
    }
}