using Almostengr.NationalWeatherService.DomainService;

namespace Almostengr.NationalWeatherService.DomainService;

public interface INwsHttpClient
{
    Task<NwsLatestObservationResponse> GetLatestObservationAsync(string stationId, CancellationToken cancellationToken);
}
