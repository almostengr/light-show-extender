using Almostengr.LightShowExtender.DomainService;

namespace Almostengr.LightShowExtender.Worker;

public class WebsiteDisplayWorker : BackgroundService
{
    private readonly IDisplayService _displayService;

    public WebsiteDisplayWorker(IDisplayService displayService)
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
