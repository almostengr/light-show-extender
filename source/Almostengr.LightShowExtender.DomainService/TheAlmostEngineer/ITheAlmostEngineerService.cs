namespace Almostengr.LightShowExtender.DomainService.TheAlmostEngineer;

public interface ITheAlmostEngineerService
{
    Task<TimeSpan> UpdateCpuTemperatureAsync();
    Task<FppLatestStatusResult> UpdateCurrentSongAsync(FppLatestStatusResult fppLatestStatusResult);
}