namespace Almostengr.FalconPiTwitter.Common.Constants
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
        public static readonly string[] NewYearHashTags = {
            $"#NewYear{DateTime.Now.AddYears(1)}", "#NewYears", "#NewYearsDay", "#NewYear", "#NewYearsEve",
        };
    }
}