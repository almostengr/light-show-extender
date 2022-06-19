using Almostengr.FalconPiTwitter.Common.Constants;

namespace Almostengr.FalconPiTwitter.Common
{
    public class AppSettings
    {
        public List<string> FppHosts { get; set; } = new();
        public int MaxHashTags { get; init; } = 3;
        public Monitoring Monitoring { get; init; } = new();
        public Twitter Twitter { get; init; } = new();
    }

    public class Twitter
    {
        public string AccessSecret { get; init; }
        public string AccessToken { get; init; }
        public string ConsumerKey { get; init; }
        public string ConsumerSecret { get; init; }
    }

    public class Monitoring
    {
        public List<string> AlarmUsernames { get; init; } = new();
        public int MaxAlarmsPerHour { get; init; } = 3;
        public double MaxCpuTemperatureC { get; init; } = 60.0;
    }
}