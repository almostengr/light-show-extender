namespace Almostengr.FalconPiPlayer.DomainService.FalconPiPlayer;

public interface IFppHttpClient
{
    Task<MediaMetaResponse> GetCurrentSongMetaDataAsync(string currentSong, CancellationToken cancellationToken);
    Task<FppStatusResponse> GetFppdStatusAsync(CancellationToken cancellationToken, string hostname = "");
    Task<FppMultiSyncSystemsResponse> GetMultiSyncSystemsAsync(CancellationToken cancellationToken);
}