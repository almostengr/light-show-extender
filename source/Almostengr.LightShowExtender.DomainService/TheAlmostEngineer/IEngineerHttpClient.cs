using Almostengr.LightShowExtender.DomainService.Common;
using Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

namespace Almostengr.LightShowExtender.DomainService.TheAlmostEngineer;

public interface IEngineerHttpClient : IBaseHttpClient
{
    Task<EngineerResponseDto> GetFirstUnplayedRequestAsync();
    Task DeleteAllSongsInQueueAsync();
    Task<EngineerSettingResponseDto> UpdateSettingAsync(EngineerSettingRequestDto engineerSettingDto);
    Task PostLatestVitalsAsync(EngineerLightShowVitalsRequestDto vitalsDto);
}