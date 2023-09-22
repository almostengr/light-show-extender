using Almostengr.LightShowExtender.Infrastructure.Common;
using Almostengr.LightShowExtender.DomainService.NwsWeather;
using Almostengr.LightShowExtender.DomainService.Common;

namespace Almostengr.LightShowExtender.Infrastructure.NwsWeather;

public sealed class NwsHttpClient : BaseHttpClient, INwsHttpClient
{
    private readonly AppSettings _appSettings;
    private readonly HttpClient _httpClient;

    public NwsHttpClient(AppSettings appSettings)
    {
        _appSettings = appSettings; 
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(GetUrlWithProtocol(_appSettings.NwsApiUrl));
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.61 Safari/537.36");
    }

    public async Task<NwsLatestObservationResponseDto> GetLatestObservation(string stationId)
    {
        string route = $"stations/{stationId}/observations/latest";
        return await HttpGetAsync<NwsLatestObservationResponseDto>(_httpClient, route);
    }
}