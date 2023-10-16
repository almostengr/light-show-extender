using Almostengr.LightShowExtender.DomainService;

namespace Almostengr.LightShowExtender.Worker;

internal sealed class WebsiteDisplayWorker : BackgroundService
{
    private readonly IExtenderService _extenderService;

    public WebsiteDisplayWorker(IExtenderService extenderService)
    {
        _extenderService = extenderService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        TimeSpan delayTime;
        while (!stoppingToken.IsCancellationRequested)
        {
            delayTime = await _extenderService.MonitorAsync();
            await Task.Delay(delayTime, stoppingToken);
        }
    }
}
