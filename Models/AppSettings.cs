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
    }

    public class AlarmSettings
    {
        public double Temperature { get; set; }
        public string TwitterUser { get; set; }
    }

    public class FppMonitorSettings
    {
        public bool PostOffline { get; set; }
        public int RefreshInterval { get; set; }
    }

    public class FalconPiPlayerSettings
    {
        [Required]
        public string FalconUri { get; set; }
    }
}