using Almostengr.FalconPiPlayer;
using Almostengr.NationalWeatherService;

namespace Almostengr.LightShowExtender.Worker;

public sealed class AppSettings
{
    public FppAppSettings FalconPlayer { get; init; } = new();
    public TwitterSettings Twitter { get; init; } = new();
    public NwsAppSettings Nws { get; init; } = new();
    public uint SongsBetweenTweets { get; init; } = 10;
    public uint ExtenderDelay { get; init; } = 5;

}