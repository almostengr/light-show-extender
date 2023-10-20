using Almostengr.Common.Utilities;
using Microsoft.Extensions.Options;

namespace Almostengr.Common.NwsWeather;

public interface INwsHttpClient
{
    Task<NwsLatestObservationResponse> GetLatestObservationAsync(string stationId, CancellationToken cancellationToken);
}

public sealed class NwsHttpClient : BaseHttpClient, INwsHttpClient
{
    public NwsHttpClient(IOptions<NwsOptions> options)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(options.Value.StationId);
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
        var result = await HttpGetAsync<NwsLatestObservationResponse>(route, cancellationToken);
        return result;
    }
}