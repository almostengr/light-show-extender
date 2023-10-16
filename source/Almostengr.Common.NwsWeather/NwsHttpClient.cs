using Almostengr.Common.Utilities;

namespace Almostengr.Common.NwsWeather;

public sealed class NwsHttpClient : INwsHttpClient
{
    private readonly HttpClient _httpClient;

    public NwsHttpClient(Uri nwsApiUrl)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = nwsApiUrl;
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.61 Safari/537.36");
    }

    public async Task<NwsLatestObservationResponseDto> GetLatestObservationAsync(string stationId)
    {
        string route = $"stations/{stationId}/observations/latest";
        HttpResponseMessage response = await _httpClient.GetAsync(route);
        response.EnsureSuccessStatusCode();
        return await HttpClientUtilities.DeserializeResponseBodyAsync<NwsLatestObservationResponseDto>(response);
    }
}