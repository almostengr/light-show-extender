using System.Text;
using Almostengr.Common.Command;
using Almostengr.FalconPiPlayer.DomainService;
using Almostengr.NationalWeatherService;
using Almostengr.NationalWeatherService.DomainService;

namespace Almostengr.LightShowExtender.DomainService;

public sealed class CheckVitalsCommandHandler : ICommandHandler<CheckVitalsCommand, CheckVitalsResponse>
{
    private readonly IFppHttpClient _fppHttpClient;
    // private readonly TwitterAppSettings _twitterSettings;
    private FppStatusResponse _previousStatus;
    private readonly INwsHttpClient _nwsHttpClient;
    private readonly NwsAppSettings _nwsAppSettings;
    private uint failCount = 0;
    private DateTime lastWeatherTime;

    public CheckVitalsCommandHandler()
    {
    }

    public async Task<CheckVitalsResponse> ExecuteAsync(CancellationToken cancellationToken, CheckVitalsCommand command)
    {
        StringBuilder tweet = new();

        FppStatusQueryHandler fppHandler = new(_fppHttpClient);
        FppStatusQuery fppQuery = new("http://127.0.0.1");
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
            // continue;
            return new CheckVitalsResponse();
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
        return new CheckVitalsResponse();
    }
}