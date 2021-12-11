using System.Collections.Generic;

namespace Almostengr.FalconPiTwitter.Settings
{
    public class AppSettings
    {
        public Twitter Twitter { get; set; }
        public RaspberryPi RaspberryPi { get; set; }
        public bool MonitorOnly { get; set; } = false;
    }

    public class RaspberryPi
    {
        public double MaxCpuTemperatureC { get; set; } = 60.0;
    }

    public class Twitter
    {
        public string ConsumerSecret { get; set; }
        public string ConsumerKey { get; set; }
        public string AccessToken { get; set; }
        public string AccessSecret { get; set; }
        public List<string> AlarmUsers { get; set; } = new();
        public int MaxAllowedAlarms { get; set; } = 3;
    }

}