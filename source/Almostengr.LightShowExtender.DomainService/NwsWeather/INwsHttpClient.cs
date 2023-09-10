namespace Almostengr.LightShowExtender.DomainService.NwsWeather;

public interface INwsHttpClient
{
    Task<NwsLatestObservationDto> GetLatestObservation(string stationId);
}