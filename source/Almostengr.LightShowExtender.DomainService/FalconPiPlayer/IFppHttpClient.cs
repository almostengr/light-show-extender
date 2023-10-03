using Almostengr.LightShowExtender.DomainService.Common;

namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public interface IFppHttpClient : IBaseHttpClient
{
    Task<FppMediaMetaResponseDto> GetCurrentSongMetaDataAsync(string currentSong);
    Task<FppStatusResponseDto> GetFppdStatusAsync();
    Task<FppStatusResponseDto> GetFppdStatusAsync(string hostname);
    Task<string> GetInsertPlaylistAfterCurrent(string playlistName);
    Task<FppMultiSyncSystemsResponseDto> GetMultiSyncSystemsAsync();
    Task<List<string>> GetSequenceListAsync();
    Task StopPlaylistGracefullyAsync();
}