using Almostengr.LightShowExtender.DomainService.TheAlmostEngineer;

namespace Almostengr.LightShowExtender.Worker;

internal sealed class TheAlmostEngineerWorker : BackgroundService
{
    private readonly ITheAlmostEngineerService _theAlmostEngineerService;

    public TheAlmostEngineerWorker(ITheAlmostEngineerService theAlmostEngineerService)
    {
        _theAlmostEngineerService = theAlmostEngineerService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        string lastSong = string.Empty;
        while (!stoppingToken.IsCancellationRequested)
        {
            var result = await _theAlmostEngineerService.UpdateCurrentSongAsync(lastSong);
            lastSong = result.lastSong;
            await Task.Delay(result.delay);
        }
    }
}
