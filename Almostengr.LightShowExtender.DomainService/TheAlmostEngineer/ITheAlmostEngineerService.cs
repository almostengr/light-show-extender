namespace Almostengr.LightShowExtender.DomainService.TheAlmostEngineer;

public interface ITheAlmostEngineerService
{
    Task<TimeSpan> UpdateCpuTemperatureAsync();
    Task<(string lastSong, TimeSpan delay)> UpdateCurrentSongAsync(string lastSong);
}