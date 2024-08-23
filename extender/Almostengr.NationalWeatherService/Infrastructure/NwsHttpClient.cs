using Almostengr.Common;
using Almostengr.NationalWeatherService.DomainService;

namespace Almostengr.NationalWeatherService.Infrastructure;

public sealed class NwsHttpClient : INwsHttpClient
{
    private readonly NwsAppSettings _nwsAppSettings;
    private readonly HttpClient _httpClient;

    public NwsHttpClient(NwsAppSettings nwsAppSettings)
    {
        _nwsAppSettings = nwsAppSettings;
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(nwsAppSettings.ApiUrl);
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("User-Agent", nwsAppSettings.UserAgent);
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