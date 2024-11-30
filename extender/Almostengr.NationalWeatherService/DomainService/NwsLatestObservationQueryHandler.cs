using Almostengr.Common.Query;

namespace Almostengr.NationalWeatherService.DomainService;

public class NwsLatestObservationQueryHandler : IQueryHandler<NwsLatestObservationResponse>
{
    private readonly INwsHttpClient _nwsHttpClient;
    private readonly NwsAppSettings _nwsAppSettings;

    public NwsLatestObservationQueryHandler(INwsHttpClient nwsHttpClient, NwsAppSettings nwsAppSettings)
    {
        _nwsHttpClient = nwsHttpClient;
        _nwsAppSettings = nwsAppSettings;
    }

    public async Task<NwsLatestObservationResponse> ExecuteAsync(CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_nwsAppSettings.StationId))
        {
            throw new ArgumentNullException(nameof(_nwsAppSettings.StationId));
        }

        return await _nwsHttpClient.GetLatestObservationAsync(_nwsAppSettings.StationId, cancellationToken);
    }
}
