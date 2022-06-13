namespace Almostengr.FalconPiTwitter.Common.Services
{
    public interface ICountDownService
    {
        Task ExecuteLightShowCountdownAsync(CancellationToken cancellationToken);
        Task ExecuteChristmasCountdownAsync(CancellationToken cancellationToken);
    }
}