using Almostengr.LightShowExtender.Infrastructure.Common;
using Almostengr.LightShowExtender.DomainService.TheAlmostEngineer;

namespace Almostengr.LightShowExtender.Infrastructure.TheAlmostEngineer;

internal sealed class EngineerHttpClient : BaseHttpClient, IEngineerHttpClient
{
    private readonly HttpClient _httpClient;
    private const string FppApiRoute = "fpp.php";

    public EngineerHttpClient()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("http://lightshow.thealmostengineer.com/");
    }

    public async Task DeleteAllSongsInQueueAsync()
    {
        await HttpDeleteAsync(_httpClient, FppApiRoute);
    }

    public async Task<EngineerResponseDto> GetFirstUnplayedRequestAsync()
    {
        return await HttpGetAsync<EngineerResponseDto>(_httpClient, FppApiRoute);
    }
    
    public async Task<EngineerSettingResponseDto> UpdateSettingAsync(EngineerSettingRequestDto engineerSettingDto)
    {
        return await HttpPutAsync<EngineerSettingRequestDto, EngineerSettingResponseDto>(_httpClient, FppApiRoute, engineerSettingDto);
    }
}