namespace Almostengr.HpLightShow.WebApi.Models;

public sealed class AppSettings
{
    public List<string> ChristmasHashTags { get; init; } = null;
    public List<string> IndependenceDayHashTags { get; init; } = null;
    public string SequenceOverride { get; set; } = null;
    public DateOnly EasterStartDate { get; init; } = new(DateTime.Now.Year, 3, 26);
    public DateOnly EasterEndDate { get; init; } = new(DateTime.Now.Year, 3, 28);
    public DateOnly FatTuesdayDate { get; init; } = new(DateTime.Now.Year, 2, 28);
    public int MonitorDelay { get; init; } = 5;

    public BlueSkySettings BlueSky { get; init; } = new();
    public TwitterSettings Twitter { get; init; } = new();
    public FalconPiPlayerSettings Fpp { get; init; } = new();

    public class FalconPiPlayerSettings
    {
        public string ApiUrl { get; init; } = "http://10.10.50.101";
        public double MaxCpuTemperatureC { get; init; } = 60.0;
    }

    public class TwitterSettings
    {
        public string AccessToken { get; init; }
        public string AccessSecret { get; init; }
        public string ConsumerSecret { get; init; }
        public string ConsumerKey { get; init; }
    }

    public class BlueSkySettings
    {
        public string Username { get; init; }
        public string Password { get; init; }
    }
}
