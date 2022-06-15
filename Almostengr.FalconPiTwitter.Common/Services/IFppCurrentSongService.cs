namespace Almostengr.FalconPiTwitter.Common.Services
{
    public interface IFppCurrentSongService
    {
        Task ExecuteCurrentSongWorkerAsync(CancellationToken cancellationToken);
    }
}