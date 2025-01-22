using Almostengr.HpLightShow.Core.FalconPiPlayer.Services;

namespace Almostengr.HpLightShow.Worker;

internal sealed class FppMonitorWorker : BackgroundService
{
    private readonly ILogger<FppMonitorWorker> _logger;
    private readonly IFppdService _fppdService;

    public FppMonitorWorker(
        ILogger<FppMonitorWorker> logger,
        IFppdService fppdService)
    {
        _logger = logger;
        _fppdService = fppdService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await _fppdService.MonitorAsync();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}