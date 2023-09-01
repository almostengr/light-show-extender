using Almostengr.LightShowExtender.DomainService.TheAlmostEngineer;

namespace Almostengr.LightShowExtender.Worker;

public class FppMonitoringWorker : BackgroundService
{
    private readonly ITheAlmostEngineerService _theAlmostEngineerService;
    public FppMonitoringWorker(ITheAlmostEngineerService theAlmostEngineerService)
    {
        _theAlmostEngineerService = theAlmostEngineerService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var delayTime = await _theAlmostEngineerService.UpdateCpuTemperatureAsync();
            await Task.Delay(delayTime, stoppingToken);
        }
    }
}
