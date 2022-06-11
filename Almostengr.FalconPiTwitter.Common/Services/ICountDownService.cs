namespace Almostengr.FalconPiTwitter.Common.Services
{
    public interface ICountDownService
    {
        Task TimeUntilChristmasAsync();
        Task TimeUntilNextLightShowAsync();
    }
}