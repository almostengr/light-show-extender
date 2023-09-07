using Almostengr.LightShowExtender.DomainService.Common;

namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public interface IFppHttpClient : IBaseHttpClient
{
    Task<FppMediaMetaDto> GetCurrentSongMetaDataAsync(string currentSong);
    Task<FppStatusDto> GetFppdStatusAsync();
    Task GetInsertPlaylistAfterCurrent(string playlistName);
}