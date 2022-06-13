namespace Almostengr.FalconPiTwitter.Common.Services
{
    public interface IFppService
    {
        Task ExecuteVitalsWorkerAsync(CancellationToken cancellationToken);
        Task ExecuteCurrentSongWorkerAsync(CancellationToken cancellationToken);
    }
}