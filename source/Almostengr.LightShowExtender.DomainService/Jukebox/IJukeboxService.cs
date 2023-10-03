namespace Almostengr.LightShowExtender.DomainService.Jukebox;

public interface IJukeboxService
{
    Task<TimeSpan> ManageRequestsAsync();
}
