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

    public async Task<NwsLatestObservationResponse> GetLatestObservationAsync(CancellationToken cancellationToken, bool forceRefresh = false)
    {
        if (string.IsNullOrWhiteSpace(_options.Value.StationId))
        {
            throw new ArgumentNullException(nameof(_options.Value.StationId));
        }

        DateTime oneHourAgo = DateTime.Now.AddHours(-1);
        if (!forceRefresh || _lastReloadTime <= oneHourAgo)
        {
            _latestObservation = await _httpClient.GetLatestObservationAsync(_options.Value.StationId, cancellationToken);
            _lastReloadTime = DateTime.Now;
        }

        return _latestObservation;
    }
}

public interface INwsService
{
    Task<NwsLatestObservationResponse> GetLatestObservationAsync(CancellationToken cancellationToken, bool forceRefresh = false);
}