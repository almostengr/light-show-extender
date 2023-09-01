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
        FppLatestStatusResult fppLatestStatusDto = new(TimeSpan.FromSeconds(5), string.Empty, string.Empty);
        while (!stoppingToken.IsCancellationRequested)
        {
            fppLatestStatusDto = await _theAlmostEngineerService.UpdateCurrentSongAsync(fppLatestStatusDto);
            await Task.Delay(fppLatestStatusDto.WorkerDelay);
        }
    }
}
