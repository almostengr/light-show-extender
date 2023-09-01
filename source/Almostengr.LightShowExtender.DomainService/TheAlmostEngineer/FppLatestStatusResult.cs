namespace Almostengr.LightShowExtender.DomainService.TheAlmostEngineer;

public sealed class FppLatestStatusResult
{
    public FppLatestStatusResult(TimeSpan workerDelay, string lastSong, string lastPlaylist)
    {
        WorkerDelay = workerDelay;
        LastSong = lastSong;
        LastPlaylist = lastPlaylist;
    }

    public TimeSpan WorkerDelay { get; init; }
    public string LastSong { get; init; }
    public string LastPlaylist { get; init; }
}