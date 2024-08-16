using Almostengr.LightShowExtender.DomainService.Common;
using Tweetinvi;

namespace Almostengr.LightShowExtender.Worker.Worker;

internal sealed class Worker : BackgroundService
{
    private readonly ITwitterClient _twitterClient;
    private readonly AppSettings _appSettings;

    public Worker(
        AppSettings appSettings,
        ITwitterClient twitterClient
    )
    {
        _twitterClient = twitterClient;
        _appSettings = appSettings;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(5));
        }
    }
}
