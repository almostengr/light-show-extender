using Almostengr.LightShowExtender.Infrastructure.Common;
using Almostengr.LightShowExtender.DomainService.TheAlmostEngineer;
using Almostengr.LightShowExtender.DomainService.Common;

namespace Almostengr.LightShowExtender.Infrastructure.TheAlmostEngineer;

public sealed class EngineerHttpClient : BaseHttpClient, IEngineerHttpClient
{
    private readonly AppSettings _appSettings;
    private readonly HttpClient _httpClient;
    private const string FPP_API_ROUTE = "fpp.php";

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
        await HttpDeleteAsync(_httpClient, FPP_API_ROUTE);
    }

    public async Task<EngineerResponseDto> GetFirstUnplayedRequestAsync()
    {
        return await HttpGetAsync<EngineerResponseDto>(_httpClient, FPP_API_ROUTE);
    }

    public async Task PostDisplayInfoAsync(EngineerLightShowDisplayRequestDto vitalsDto)
    {
        await HttpPostAsync<EngineerLightShowDisplayRequestDto>(_httpClient, FPP_API_ROUTE, vitalsDto);
    }
}