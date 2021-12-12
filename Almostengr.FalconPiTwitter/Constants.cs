using System;

namespace Almostengr.FalconPiTwitter.Constants
{
    public sealed class TwitterConstants
    {
        public static readonly int TweetCharacterLimit = 280;
        public static readonly string[] ChristmasHashTags = {
            "#LightShow", "#AnimatedLights", "#LedLighting",
            "#ChristmasLightShow", "#ChristmasLights", "#Christmas", "#Christmas", "#ChristmasSeason",
            "#ChristmasTime", "#ChristmasDecorations", "#ChristmasSpirit", "#ChristmasMagic",
            "#ChristmasFun", $"#Christmas{DateTime.Now.Year}", "#MerryChristmas", "#ChristmasMusic",
            "#ChristmasLighting",
            "#HolidayLightShow", "#HolidayLightShows", "#HolidayLights", "#HappyHolidays",
            "#HolidayLighting"};
    }

    public sealed class PlaylistIgnoreName
    {
        public static readonly string Offline = "offline";
        public static readonly string Testing = "testing";
    }

    public sealed class AppConstants
    {
        public static readonly Uri Localhost = new Uri("http://localhost");
    }

    public sealed class FppMode
    {
        public static readonly string Standalone = "standalone";
        public static readonly string Master = "master";
        public static readonly string Remote = "remote";
    }

    public sealed class DelaySeconds
    {
        public static readonly int Short = 15;
        public static readonly int Long = 300;
        public static readonly int ExtraLong = 900;
    }

    public sealed class ExceptionMessage
    {
        public static readonly string NoInternetConnection = "Are you connected to internet? Is FFPPd running? ";
        public static readonly string NullReference = "Null Exception occurred. ";
    }
}