using Almostengr.LightShowExtender.Infrastructure.Common;
using Almostengr.LightShowExtender.DomainService.TheAlmostEngineer;
using Almostengr.LightShowExtender.DomainService.Common;

namespace Almostengr.LightShowExtender.Infrastructure.TheAlmostEngineer;

public sealed class EngineerHttpClient : BaseHttpClient, IEngineerHttpClient
{
    private readonly AppSettings _appSettings;
    private readonly HttpClient _httpClient;
    private const string FppApiRoute = "fpp.php";

    public EngineerHttpClient(AppSettings appSettings)
    {
        _appSettings = appSettings;
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(GetUrlWithProtocol(_appSettings.FrontEnd.ApiUrl));
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("X-Auth-Token", _appSettings.FrontEnd.ApiKey);
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

    public async Task PostLatestVitalsAsync(EngineerLightShowVitalsRequestDto vitalsDto)
    {
        await HttpPostAsync<EngineerLightShowVitalsRequestDto>(_httpClient, FppApiRoute, vitalsDto);
    }
}