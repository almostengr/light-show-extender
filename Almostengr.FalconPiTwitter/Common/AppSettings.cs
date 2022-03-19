using System.Collections.Generic;

namespace Almostengr.FalconPiTwitter.Common
{
    public class AppSettings
    {
        public List<string> FppHosts { get; set; } = new();
        public int MaxHashTags { get; set; } = 3;
        public Monitoring Monitoring { get; set; } = new();
        public Twitter Twitter { get; set; } = new();
    }

    public class Twitter
    {
        public string AccessSecret { get; set; }
        public string AccessToken { get; set; }
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
    }

    public class Monitoring
    {
        public List<string> AlarmUsernames { get; set; } = new();
        public int MaxAlarmsPerHour { get; set; } = 3;
        public double MaxCpuTemperatureC { get; set; } = 60.0;
    }
}