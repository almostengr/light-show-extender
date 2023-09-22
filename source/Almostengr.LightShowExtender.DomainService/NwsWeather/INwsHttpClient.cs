namespace Almostengr.LightShowExtender.DomainService.NwsWeather;

public interface INwsHttpClient
{
    Task<NwsLatestObservationResponseDto> GetLatestObservationAsync(string stationId);
}