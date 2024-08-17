using Almostengr.Extensions;
using Almostengr.NationalWeatherService.DomainService;

namespace Almostengr.NationalWeatherService.Infrastructure;

public sealed class NwsHttpClient : INwsHttpClient
{
    private readonly NwsAppSettings _nwsOptions;
    private readonly HttpClient _httpClient;

    public NwsHttpClient(NwsAppSettings nwsOptions)
    {
        _nwsOptions = nwsOptions;
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(nwsOptions.ApiUrl);
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("User-Agent", nwsOptions.UserAgent);
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