using Almostengr.LightShowExtender.DomainService.Common;

namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public interface IFppHttpClient : IBaseHttpClient
{
    Task<FppMediaMetaDto> GetCurrentSongMetaDataAsync(string currentSong);
    Task<FppStatusDto> GetFppdStatusAsync(string address);
    Task GetInsertPlaylistAfterCurrent(string playlistName);
}