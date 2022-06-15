namespace Almostengr.FalconPiTwitter.Common.Services
{
    public interface IFppVitalsService
    {
        Task ExecuteVitalsWorkerAsync(CancellationToken cancellationToken);
    }
}