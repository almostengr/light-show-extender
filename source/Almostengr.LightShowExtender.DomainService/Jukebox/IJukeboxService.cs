namespace Almostengr.LightShowExtender.DomainService.Jukebox;

public interface IJukeboxService
{
    Task<LatestJukeboxStateDto> UpdateCurrentSongAsync(LatestJukeboxStateDto latestJukeboxStateDto);
}