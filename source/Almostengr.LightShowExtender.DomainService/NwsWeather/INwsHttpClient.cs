namespace Almostengr.LightShowExtender.DomainService.NwsWeather;

public interface INwsHttpClient
{
    public Task<NwsLatestObservationDto> GetLatestObservation(string stationId);
}