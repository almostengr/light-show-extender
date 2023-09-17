namespace Almostengr.LightShowExtender.DomainService.NwsWeather;

public interface INwsHttpClient
{
    Task<NwsLatestObservationResponseDto> GetLatestObservation(string stationId);
}