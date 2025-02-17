using Almostengr.Common.DomainServices.Results;
using Almostengr.HpLightShow.Core.FalconPiPlayer.DomainServices;
using Almostengr.HpLightShow.Core.FalconPiPlayer.DomainServices.Interfaces;

namespace Almostengr.HpLightShow.Worker;

internal sealed class FppMonitorWorker : BackgroundService
{
    private readonly IFppMonitorService _fppdService;
    private readonly ILogger<FppMonitorWorker> _logger;

    public FppMonitorWorker(
        IFppMonitorService fppdService,
        ILogger<FppMonitorWorker> logger
        )
    {
        _fppdService = fppdService;
        _logger = logger;
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