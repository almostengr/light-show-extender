using Almostengr.Common.Query;

namespace Almostengr.NationalWeatherService.DomainService;

public class GetLatestObservationQueryHandler : IQueryHandler<NwsLatestObservationResponse>
{
    private readonly INwsHttpClient _nwsHttpClient;
    private readonly NwsAppSettings _options;

    public GetLatestObservationQueryHandler(INwsHttpClient nwsHttpClient, NwsAppSettings options)
    {
        _nwsHttpClient = nwsHttpClient;
        _options = options;
    }

    public async Task<NwsLatestObservationResponse> ExecuteAsync(CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_options.StationId))
        {
            throw new ArgumentNullException(nameof(_options.StationId));
        }

        return await _nwsHttpClient.GetLatestObservationAsync(_options.StationId, cancellationToken);
    }
}
