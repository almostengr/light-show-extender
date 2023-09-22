namespace Almostengr.LightShowExtender.DomainService.Jukebox;

public interface IJukeboxService
{
    Task DelayBetweenRequestsAsync();
    Task GetLatestJukeboxRequestAsync();
    Task UpdateJukeboxAsync();
}