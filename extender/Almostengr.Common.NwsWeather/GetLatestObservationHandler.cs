using Almostengr.Extensions;
using Almostengr.Extensions.Logging;

namespace Almostengr.Common.NwsWeather;

public class GetLatestObservationHandler
{
    private readonly INwsHttpClient _nwsHttpClient;
    private readonly ILoggingService<GetLatestObservationHandler> _loggingService;

    public GetLatestObservationHandler(INwsHttpClient nwsHttpClient, ILoggingService<GetLatestObservationHandler> loggingService)
    {
        _nwsHttpClient = nwsHttpClient;
        _loggingService = loggingService;
    }

    public async Task<NwsLatestObservationResponse> Handle(string stationId, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(stationId))
            {
                throw new ArgumentNullException(nameof(stationId));
            }

            return await _nwsHttpClient.GetLatestObservationAsync(stationId, cancellationToken);
        }
        catch (Exception ex)
        {
            _loggingService.Error(ex.Message);
            return null;
        }
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
