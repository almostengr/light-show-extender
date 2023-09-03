using Almostengr.LightShowExtender.DomainService.Common;

namespace Almostengr.LightShowExtender.DomainService.TheAlmostEngineer;

public interface IEngineerHttpClient : IBaseHttpClient
{
    Task<EngineerResponseDto> GetFirstUnplayedRequestAsync();
    Task DeleteAllSongsInQueueAsync();
    Task<EngineerSettingResponseDto> UpdateSettingAsync(EngineerSettingRequestDto engineerSettingDto);
}