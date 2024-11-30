using System.Text;
using Almostengr.FalconPiPlayer.DomainService;
using Almostengr.LightShowExtender.DomainService.Twitter;
using Almostengr.NationalWeatherService;
using Almostengr.NationalWeatherService.DomainService;

namespace Almostengr.LightShowExtender.Worker;

internal sealed class VitalsWorker : BackgroundService
{
    private readonly IFppHttpClient _fppHttpClient;
    // private readonly TwitterAppSettings _twitterSettings;
    private FppStatusResponse _previousStatus;
    private ILogger<VitalsWorker> _logger;
    private readonly INwsHttpClient _nwsHttpClient;
    private readonly NwsAppSettings _nwsAppSettings;
    private uint failCount = 0;
    private DateTime lastWeatherTime;

    public VitalsWorker(
        IFppHttpClient fppHttpClient,
        ILogger<VitalsWorker> logger,
        // TwitterAppSettings twitterSettings,
        INwsHttpClient nwsHttpClient,
        NwsAppSettings nwsAppSettings
        )
    {
        _fppHttpClient = fppHttpClient;
        // _twitterSettings = twitterSettings;
        _logger = logger;
        _nwsHttpClient = nwsHttpClient;
        _nwsAppSettings = nwsAppSettings;
        _previousStatus = new();
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                StringBuilder tweet = new();

                FppStatusQueryHandler fppHandler = new(_fppHttpClient);
                FppStatusQuery fppQuery = new("http://localhost");
                FppStatusResponse result = await fppHandler.ExecuteAsync(cancellationToken, fppQuery);

                if (_previousStatus.Status != FppStatusTypes.Idle && result.Status == FppStatusTypes.Idle)
                {
                    tweet.Append("Show is offline");
                }
                else if (_previousStatus.Status == FppStatusTypes.Idle && result.Status != FppStatusTypes.Idle)
                {
                    tweet.Append("Show is online");
                }

                if (result.Status == FppStatusTypes.Idle)
                {
                    await Task.Delay(TimeSpan.FromSeconds(15));
                    continue;
                }

                CpuTemperatureQueryHandler temperatureHandler = new CpuTemperatureQueryHandler(_fppHttpClient);
                string temperatures = await temperatureHandler.ExecuteAsync(cancellationToken);
                tweet.Append(temperatures);

                NwsLatestObservationQueryHandler weatherHandler = new(_nwsHttpClient, _nwsAppSettings);
                NwsLatestObservationResponse weather = await weatherHandler.ExecuteAsync(cancellationToken);

                // if (tweet.Length > 0)
                // {
                //     PostTweetCommandHandler tweetHandler = new(_twitterSettings);
                //     PostTweetCommand tweetCommand = new(tweet.ToString());
                //     await tweetHandler.ExecuteAsync(cancellationToken, tweetCommand);
                // }

                _previousStatus = result;
                failCount = 0;
            }
            catch (Exception exception)
            {
                if (failCount < 5)
                {
                    failCount++;
                    _logger.LogError(exception.Message);
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(15));
        }
    }
}