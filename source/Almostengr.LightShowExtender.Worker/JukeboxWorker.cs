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
        TimeSpan delayTime;
        while (!stoppingToken.IsCancellationRequested)
        {
            delayTime = await _jukeboxService.ManageRequestsAsync();
            await Task.Delay(delayTime);
        }
    }
}
