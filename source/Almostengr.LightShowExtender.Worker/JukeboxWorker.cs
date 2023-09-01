using Almostengr.LightShowExtender.DomainService.Jukebox;

namespace Almostengr.LightShowExtender.Worker;

internal sealed class JukeboxWorker : BackgroundService
{
    private readonly IJukeboxService _jukeboxService;

    public JukeboxWorker(IJukeboxService jukeboxService)
    {
        _jukeboxService = jukeboxService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        LatestJukeboxStateDto latestJukeboxDto = new(TimeSpan.FromSeconds(5), string.Empty, string.Empty);
        while (!stoppingToken.IsCancellationRequested)
        {
            latestJukeboxDto = await _jukeboxService.UpdateCurrentSongAsync(latestJukeboxDto);
            await Task.Delay(latestJukeboxDto.WorkerDelay);
        }
    }
}
