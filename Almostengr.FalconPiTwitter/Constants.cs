using System;

namespace Almostengr.FalconPiTwitter.Constants
{
    public sealed class TwitterConstants
    {
        public const int TweetCharacterLimit = 280;
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
        public const string Offline = "offline";
        public const string Testing = "testing";
    }

    public sealed class AppConstants
    {
        public static readonly Uri Localhost = new Uri("http://localhost");
    }

    public sealed class FppMode
    {
        public const string Standalone = "standalone";
        public const string Master = "master";
        public const string Remote = "remote";
    }

    public sealed class DelaySeconds
    {
        public const int Short = 15;
        public const int Long = 300;
        public const int ExtraLong = 900;
    }

    public sealed class ExceptionMessage
    {
        public const string NoInternetConnection = "Are you connected to internet? Is FPPd running? ";
        public const string NullReference = "Null Exception occurred. ";
    }
}