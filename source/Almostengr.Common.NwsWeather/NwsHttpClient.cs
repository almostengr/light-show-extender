using Almostengr.Extensions;
using Microsoft.Extensions.Options;

namespace Almostengr.Common.NwsWeather;

public interface INwsHttpClient
{
    Task<NwsLatestObservationResponse> GetLatestObservationAsync(string stationId, CancellationToken cancellationToken);
}

public sealed class NwsHttpClient : INwsHttpClient
{
    private readonly HttpClient _httpClient;

    public NwsHttpClient(IOptions<NwsOptions> options)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(options.Value.ApiUrl);
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("User-Agent", options.Value.UserAgent);
    }

    public async Task<NwsLatestObservationResponse> GetLatestObservationAsync(string stationId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(stationId))
        {
            throw new ArgumentNullException(nameof(stationId));
        }

        string route = $"stations/{stationId}/observations/latest";
        return await _httpClient.GetAsync<NwsLatestObservationResponse>(route, cancellationToken);
    }
}