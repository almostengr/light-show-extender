using System.Collections.Generic;

namespace Almostengr.FalconPiTwitter.Settings
{
    public class AppSettings
    {
        public Twitter Twitter { get; set; } = new();
        public Monitoring Monitoring { get; set; } = new();
        public bool CountdownEnabled { get; set; } = false;
        public int MaxHashTags { get; set; } = 3;
        public List<string> FppHosts { get; set; } = new();
    }

    public class Twitter
    {
        public string ConsumerSecret { get; set; }
        public string ConsumerKey { get; set; }
        public string AccessToken { get; set; }
        public string AccessSecret { get; set; }
    }

    public class Monitoring
    {
        public List<string> AlarmUsernames { get; set; } = new();
        public int MaxAlarmsPerHour { get; set; } = 3;
        public double MaxCpuTemperatureC { get; set; } = 60.0;
    }
}