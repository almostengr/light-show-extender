namespace Almostengr.LightShowExtender.DomainService.Jukebox;

public sealed class LatestJukeboxStateDto
{
    public LatestJukeboxStateDto(TimeSpan workerDelay, string lastSong, string lastPlaylist)
    {
        WorkerDelay = workerDelay;
        LastSong = lastSong;
        LastPlaylist = lastPlaylist;
    }

    public TimeSpan WorkerDelay { get; init; }
    public string LastSong { get; init; }
    public string LastPlaylist { get; init; }
}