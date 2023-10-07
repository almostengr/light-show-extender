using Almostengr.LightShowExtender.DomainService;

namespace Almostengr.LightShowExtender.Worker;

internal sealed class WebsiteDisplayWorker : BackgroundService
{
    private readonly IExtenderService _displayService;

    public WebsiteDisplayWorker(IExtenderService displayService)
    {
        _displayService = displayService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        TimeSpan delayTime = TimeSpan.FromSeconds(30);
        while (!stoppingToken.IsCancellationRequested)
        {
            delayTime = await _displayService.UpdateWebsiteDisplayAsync();
            await Task.Delay(delayTime, stoppingToken);
        }
    }
}
