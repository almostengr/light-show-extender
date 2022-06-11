namespace Almostengr.FalconPiTwitter.Common.Services
{
    public interface ICountDownService
    {
        Task TimeUntilChristmasAsync();
        Task TimeUntilNextLightShowAsync();
        Task ExecuteLightShowCountdownAsync(CancellationToken cancellationToken);
        Task ExecuteChristmasCountdownAsync(CancellationToken cancellationToken);
    }
}