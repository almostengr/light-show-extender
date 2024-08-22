namespace Almostengr.FalconPiPlayer.DomainService;

public interface IFppHttpClient
{
    Task<MediaMetaResponse> GetCurrentSongMetaDataAsync(string currentSong, CancellationToken cancellationToken);
    Task<FppStatusResponse> GetFppdStatusAsync(CancellationToken cancellationToken, string hostname = "");
    Task<MultiSyncSystemsQueryResponse> GetMultiSyncSystemsAsync(CancellationToken cancellationToken);
}