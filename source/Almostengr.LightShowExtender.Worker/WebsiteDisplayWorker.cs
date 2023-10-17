using Almostengr.LightShowExtender.DomainService;

namespace Almostengr.LightShowExtender.Worker;

internal sealed class WebsiteDisplayWorker : BackgroundService
{
    private readonly IExtenderService _extenderService;

    public WebsiteDisplayWorker(IExtenderService extenderService)
    {
        _extenderService = extenderService;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        TimeSpan delayTime;
        while (!cancellationToken.IsCancellationRequested)
        {
            delayTime = await _extenderService.MonitorAsync(cancellationToken);
            await Task.Delay(delayTime, cancellationToken);
        }
    }
}
