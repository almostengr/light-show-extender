namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public interface IFppHttpClient
{
    Task<FppStatusResponse> GetFppdStatusAsync(CancellationToken cancellationToken, string hostname = "");
    Task<FppMultiSyncSystemsResponse> GetMultiSyncSystemsAsync(CancellationToken cancellationToken);
    Task StopPlaylistGracefullyAsync(CancellationToken cancellationToken);
}