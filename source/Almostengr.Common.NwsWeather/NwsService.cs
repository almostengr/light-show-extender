using Microsoft.Extensions.Options;

namespace Almostengr.Common.NwsWeather;

public sealed class NwsService : INwsService
{
    private readonly INwsHttpClient _httpClient;
    private DateTime _lastReloadTime;
    private NwsLatestObservationResponse _latestObservation;
    private IOptions<NwsOptions> _options;

    public NwsService(INwsHttpClient httpClient, IOptions<NwsOptions> options)
    {
        _httpClient = httpClient;
        _lastReloadTime = DateTime.Now.AddHours(-2);
        _latestObservation = new();
        _options = options;
    }

    public async Task<NwsLatestObservationResponse> GetLatestObservationAsync(string stationId, CancellationToken cancellationToken, bool forceRefresh = false)
    {
        if (string.IsNullOrWhiteSpace(stationId))
        {
            throw new ArgumentNullException(nameof(stationId));
        }

        TimeSpan timeDifference =  DateTime.Now - _lastReloadTime;
        if (forceRefresh || timeDifference.Hours >= 1)
        {
            _latestObservation = await _httpClient.GetLatestObservationAsync(stationId, cancellationToken);
            _lastReloadTime = DateTime.Now;
        }

        return _latestObservation;
    }
}

public interface INwsService
{
    Task<NwsLatestObservationResponse> GetLatestObservationAsync(string stationId, CancellationToken cancellationToken, bool forceRefresh = false);
}