using Almostengr.LightShowExtender.DomainService.Common;

namespace Almostengr.LightShowExtender.DomainService.TheAlmostEngineer;

public interface ITaeHttpClient : IBaseHttpClient
{
    Task UpdateSettingAsync(TaeSettingDto currentSongDto);
    Task<TaeResponseDto> GetFirstUnplayedRequestAsync();
    Task DeleteAllSongsInQueueAsync();
}