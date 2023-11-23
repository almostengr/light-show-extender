using Almostengr.Extensions;
using Almostengr.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Almostengr.Common.NwsWeather;

public class GetLatestObservationHandler : IQueryHandler<NwsLatestObservationResponse>
{
    private readonly INwsHttpClient _nwsHttpClient;
    private readonly ILoggingService<GetLatestObservationHandler> _loggingService;
    private readonly IOptions<NwsOptions> _options;

    public GetLatestObservationHandler(INwsHttpClient nwsHttpClient, IOptions<NwsOptions> options, ILoggingService<GetLatestObservationHandler> loggingService)
    {
        _nwsHttpClient = nwsHttpClient;
        _loggingService = loggingService;
        _options = options;
    }

    public async Task<NwsLatestObservationResponse> ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_options.Value.StationId))
            {
                throw new ArgumentNullException(nameof(_options.Value.StationId));
            }

            return await _nwsHttpClient.GetLatestObservationAsync(_options.Value.StationId, cancellationToken);
        }
        catch (Exception ex)
        {
            _loggingService.Error(ex.Message);
            return null!;
        }
    }
}

public sealed class NwsLatestObservationResponse : IQueryResponse
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
