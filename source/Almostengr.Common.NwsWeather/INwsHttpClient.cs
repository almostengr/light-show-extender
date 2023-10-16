namespace Almostengr.Common.NwsWeather;

public interface INwsHttpClient
{
    Task<NwsLatestObservationResponseDto> GetLatestObservationAsync(string stationId);
}