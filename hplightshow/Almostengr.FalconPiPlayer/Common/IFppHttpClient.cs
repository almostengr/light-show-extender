namespace Almostengr.FalconPiPlayer.DomainService;

public interface IFppHttpClient
{
    Task<FppStatusResult> GetFppdStatusAsync();
    Task<MultiSyncSystemsQueryResponse> GetMultiSyncSystemsAsync();
    Task<string> StartPlaylistAsync(string playlist);
}