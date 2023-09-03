using Almostengr.LightShowExtender.Infrastructure.Common;
using Almostengr.LightShowExtender.DomainService.NwsWeather;

namespace Almostengr.LightShowExtender.Infrastructure.NwsWeather;

internal sealed class NwsHttpClient : BaseHttpClient, INwsHttpClient
{
    private readonly HttpClient _httpClient;

    public NwsHttpClient()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://api.weather.gov/");
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.61 Safari/537.36");
    }

    public async Task<NwsLatestObservationDto> GetLatestObservation(string stationId)
    {
        string route = $"stations/{stationId}/observations/latest";
        return await HttpGetAsync<NwsLatestObservationDto>(_httpClient, route);
    }
}