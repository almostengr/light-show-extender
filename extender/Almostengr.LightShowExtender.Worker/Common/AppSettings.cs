using Almostengr.FalconPiPlayer.Common;
using Almostengr.NationalWeatherService;

namespace Almostengr.LightShowExtender.DomainService.Common;

public sealed class AppSettings
{
    public FppAppSettings FalconPlayer { get; init; } = new();
    public TwitterSettings Twitter { get; init; } = new();
    public NwsOptions NwsAppSettings { get; init; } = new();
    public uint SongsBetweenTweets { get; init; } = 10;
    public uint ExtenderDelay { get; init; } = 5;

    public sealed class TwitterSettings
    {
        public string AccessSecret { get; init; } = string.Empty;
        public string AccessToken { get; init; } = string.Empty;
        public string ConsumerKey { get; init; } = string.Empty;
        public string ConsumerSecret { get; init; } = string.Empty;
        public uint HashTagCount { get; init; } = 3;
    }
}