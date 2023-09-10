namespace Almostengr.LightShowExtender.DomainService.Jukebox;

public interface IJukeboxService
{
    Task GetLatestJukeboxRequest();
    Task<PreviousJukeboxStateDto> UpdateJukeboxAsync(PreviousJukeboxStateDto latestJukeboxStateDto);
}