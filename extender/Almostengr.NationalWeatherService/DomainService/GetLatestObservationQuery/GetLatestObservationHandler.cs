using Almostengr.Extensions;
using Microsoft.Extensions.Options;

namespace Almostengr.NationalWeatherService.DomainService;

public class GetLatestObservationQueryHandler : IQueryHandler<NwsLatestObservationResponse>
{
    private readonly INwsHttpClient _nwsHttpClient;
    private readonly IOptions<NwsOptions> _options;

    public GetLatestObservationQueryHandler(INwsHttpClient nwsHttpClient, IOptions<NwsOptions> options)
    {
        _nwsHttpClient = nwsHttpClient;
        _options = options;
    }

    public async Task<NwsLatestObservationResponse> ExecuteAsync(CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_options.Value.StationId))
        {
            throw new ArgumentNullException(nameof(_options.Value.StationId));
        }

        return await _nwsHttpClient.GetLatestObservationAsync(_options.Value.StationId, cancellationToken);
    }
}
