using Almostengr.Extensions;
using Almostengr.Extensions.Logging;

namespace Almostengr.Common.NwsWeather;

public class GetLatestObservationHandler
{
    private readonly INwsHttpClient _nwsHttpClient;
    private readonly ILoggingService<GetLatestObservationHandler> _loggerService;

    public GetLatestObservationHandler(INwsHttpClient nwsHttpClient, ILoggingService<GetLatestObservationHandler> loggerService)
    {
        _nwsHttpClient = nwsHttpClient;
        _loggerService = loggerService;
    }

    public async Task<NwsLatestObservationResponse> Handle(string stationId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(stationId))
        {
            throw new ArgumentNullException(nameof(stationId));
        }

        return await nwsHttpClient.GetLatestObservationAsync(stationId, cancellationToken);
    }
}

public sealed class NwsLatestObservationResponse : BaseResponse
{
    public NwsLatestObservationProperties Properties { get; init; } = new();

    public sealed class NwsLatestObservationProperties
    {
        public NwsLatestObservationTemperature Temperature { get; init; } = new();
        public NwsLatestObservationWindChill WindChill { get; init; } = new();

        public sealed class NwsLatestObservationWindChill
        {
            public float? Value { get; init; } = null;
        }

        public sealed class NwsLatestObservationTemperature
        {
            public float? Value { get; init; } = null;
        }
    }
}
