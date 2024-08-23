using System.Text;
using Almostengr.FalconPiPlayer.DomainService;
using Almostengr.LightShowExtender.DomainService.Twitter;
using Almostengr.NationalWeatherService.DomainService;
using Tweetinvi;

namespace Almostengr.LightShowExtender.Worker.Worker;

internal sealed class Worker : BackgroundService
{
    private readonly ITwitterClient _twitterClient;
    private readonly AppSettings _appSettings;
    private readonly IFppHttpClient _fppHttpClient;
    private readonly INwsHttpClient _nwsHttpClient;
    private readonly ILogger<Worker> _logger;

    public Worker(
        AppSettings appSettings,
        ITwitterClient twitterClient,
        IFppHttpClient fppHttpClient,
        INwsHttpClient nwsHttpClient,
        ILogger<Worker> logger
    )
    {
        _twitterClient = twitterClient;
        _appSettings = appSettings;
        _fppHttpClient = fppHttpClient;
        _nwsHttpClient = nwsHttpClient;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        uint failCount = 0;
        const uint IDLE_STATUS_ID = 0;
        FppStatusResponse previousStatus = new();
        uint taskDelay = _appSettings.ExtenderDelay;

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var latestStatus = await _fppHttpClient.GetFppdStatusAsync(cancellationToken, _appSettings.FalconPlayer.ApiUrl);

                StringBuilder tweet = new StringBuilder();
                if (previousStatus.Status == IDLE_STATUS_ID && latestStatus.Status != IDLE_STATUS_ID)
                {
                    await ShowStartupAsync(cancellationToken);
                    tweet.Append("The show is online. ");
                }
                else if (previousStatus.Status != IDLE_STATUS_ID && latestStatus.Status == IDLE_STATUS_ID)
                {
                    await ShowShutDownAsync(cancellationToken);
                    tweet.Append("The show is offline. ");
                }

                if (latestStatus.Status == IDLE_STATUS_ID)
                {
                    previousStatus = latestStatus;
                    failCount = 0;
                    await Task.Delay(TimeSpan.FromSeconds(_appSettings.ExtenderDelay));
                    continue;
                }

                var temperatureHandler = new CpuTemperatureQueryHandler(_fppHttpClient);
                var temperatures = await temperatureHandler.ExecuteAsync(cancellationToken);
                tweet.Append(temperatures);

                var weatherHandler = new GetLatestObservationQueryHandler(_nwsHttpClient, _appSettings.Nws);
                var weather = weatherHandler.ExecuteAsync(cancellationToken);

                if (tweet.Length > 0)
                {
                    var updateCommand = new PostTweetCommand(tweet.ToString());
                    var tweetHandler = new PostTweetHandler(_appSettings.Twitter);
                    await tweetHandler.ExecuteAsync(cancellationToken, updateCommand);
                    // _logger.LogInformation(tweet.ToString() + DateTime.Now);
                }

                previousStatus = latestStatus;
                failCount = 0;
                await Task.Delay(TimeSpan.FromSeconds(_appSettings.ExtenderDelay));
            }
            catch (Exception ex)
            {
                if (failCount < 5)
                {
                    _logger.LogError(ex.Message);
                    failCount++;
                }

                await Task.Delay(TimeSpan.FromSeconds(_appSettings.ExtenderDelay * failCount));
            }
        }
    }

    private async Task ShowShutDownAsync(CancellationToken cancellationToken)
    {
        // turn on driveway lights
        // turn off live stream
        await Task.CompletedTask;
    }

    private async Task ShowStartupAsync(CancellationToken cancellationToken)
    {
        // turn off driveway lights
        // turn on live stream
        await Task.CompletedTask;
    }
}
