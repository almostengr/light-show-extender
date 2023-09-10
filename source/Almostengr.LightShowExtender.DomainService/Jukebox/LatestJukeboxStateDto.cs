namespace Almostengr.LightShowExtender.DomainService.Jukebox;

public sealed class PreviousJukeboxStateDto
{
    public PreviousJukeboxStateDto(TimeSpan workerDelay, string lastSong, string statusName)
    {
        WorkerDelay = workerDelay;
        LastSong = lastSong;
        StatusName = statusName;
    }

    public TimeSpan WorkerDelay { get; init; }
    public string LastSong { get; init; }
    public string StatusName { get; init; }
}