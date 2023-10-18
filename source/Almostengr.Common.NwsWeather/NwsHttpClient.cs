using Almostengr.Common.Utilities;

namespace Almostengr.Common.NwsWeather;

public interface INwsHttpClient
{
    Task<NwsLatestObservationResultDto> GetLatestObservationAsync(string stationId, CancellationToken cancellationToken);
}

public sealed class NwsHttpClient : BaseHttpClient, INwsHttpClient
{
    public NwsHttpClient(string nwsApiUrl)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(nwsApiUrl);
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.61 Safari/537.36");
    }

    public async Task<NwsLatestObservationResultDto> GetLatestObservationAsync(string stationId, CancellationToken cancellationToken)
    {
        string route = $"stations/{stationId}/observations/latest";
        var result = await HttpGetAsync<NwsLatestObservationResultDto>(route, cancellationToken);
        return result;
    }
}