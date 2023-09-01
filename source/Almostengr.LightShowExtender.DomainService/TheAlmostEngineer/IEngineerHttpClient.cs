using Almostengr.LightShowExtender.DomainService.Common;

namespace Almostengr.LightShowExtender.DomainService.TheAlmostEngineer;

public interface IEngineerHttpClient : IBaseHttpClient
{
    Task UpdateSettingAsync(EngineerSettingDto currentSongDto);
    Task<EngineerResponseDto> GetFirstUnplayedRequestAsync();
    Task DeleteAllSongsInQueueAsync();
}