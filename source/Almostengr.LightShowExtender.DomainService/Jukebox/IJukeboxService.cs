namespace Almostengr.LightShowExtender.DomainService.Jukebox;

public interface IJukeboxService
{
    Task DelayBetweenRequestsAsync();
    Task GetLatestJukeboxRequest();
    Task UpdateJukeboxAsync();
}