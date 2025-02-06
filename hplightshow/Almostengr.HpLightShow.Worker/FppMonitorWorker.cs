using Almostengr.Common.OperationResult;
using Almostengr.HpLightShow.Core.FalconPiPlayer.Resources;
using Almostengr.HpLightShow.Core.FalconPiPlayer.Services;

namespace Almostengr.HpLightShow.Worker;

internal sealed class FppMonitorWorker : BackgroundService
{
    private readonly ILogger<FppMonitorWorker> _logger;
    private readonly IFppMonitorService _fppdService;

    public FppMonitorWorker(
        ILogger<FppMonitorWorker> logger,
        IFppMonitorService fppdService
        )
    {
        _logger = logger;
        _fppdService = fppdService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            FppMonitorResource resource = new();
            Result<FppMonitorResource> result = await _fppdService.ExecuteAsync(resource);
            if (result.Failed)
            {
                result.Errors.ToList().ForEach(e => _logger.LogError(e));
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}