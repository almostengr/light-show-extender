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
        while (!stoppingToken.IsCancellationRequested)
        {
            await _jukeboxService.UpdateJukeboxAsync();
            await _jukeboxService.GetLatestJukeboxRequest();
            await _jukeboxService.DelayBetweenRequestsAsync();
        }
    }
}
