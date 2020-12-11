using System.ComponentModel.DataAnnotations;

namespace Almostengr.FalconPiMonitor
{
    public class AppSettings
    {
        [Required]
        public TwitterSettings TwitterSettings { get; set; }
        public AlarmSettings AlarmSettings { get; set; }
        public FppShow FppShow { get; set; }
        public FalconPiSettings FalconPiSettings { get; set; }
    }

    public class FalconPiSettings
    {
        [Required]
        public string FalconUri { get; set; }
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
        public string AlarmUser { get; set; }
    }

    public class FppShow
    {
        public bool PostOffline { get; set; }
    }
}