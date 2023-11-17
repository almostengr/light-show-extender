namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public interface IFppHttpClient
{
    Task<FppMediaMetaResponse> GetCurrentSongMetaDataAsync(string currentSong, CancellationToken cancellationToken);
    Task<FppStatusResponse> GetFppdStatusAsync(CancellationToken cancellationToken, string hostname = "");
    Task<string> GetInsertPlaylistAfterCurrent(string playlistName, CancellationToken cancellationToken);
    Task<FppMultiSyncSystemsResponse> GetMultiSyncSystemsAsync(CancellationToken cancellationToken);
    Task<List<string>> GetSequenceListAsync(CancellationToken cancellationToken);
    Task StopPlaylistGracefullyAsync(CancellationToken cancellationToken);
}