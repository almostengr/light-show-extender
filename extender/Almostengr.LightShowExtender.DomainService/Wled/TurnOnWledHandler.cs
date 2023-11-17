using Almostengr.Extensions;
using Almostengr.Extensions.Logging;
using Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

namespace Almostengr.LightShowExtender.DomainService.Wled;

public sealed class TurnOnWledHandler: IQueryHandler<FppMultiSyncSystemsResponse.FppSystem, WledJsonResponse>
{
    private readonly IWledHttpClient _wledHttpClient;
    private readonly ILoggingService<TurnOnWledHandler> _loggingService;

    public TurnOnWledHandler(
        IWledHttpClient wledHttpClient,
        ILoggingService<TurnOnWledHandler> loggingService)
    {
        _wledHttpClient = wledHttpClient;
        _loggingService = loggingService;
    }

    public async Task<WledJsonResponse> ExecuteAsync(FppMultiSyncSystemsResponse.FppSystem system, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(system.Address))
            {
                throw new ArgumentNullException(nameof(system));
            }

            var request = new WledJsonStateRequest(true);
            return await _wledHttpClient.PostStateAsync(system.Address, request, cancellationToken);
        }
        catch (Exception ex)
        {
            _loggingService.Error(ex.Message);
            return null;
        }
    }

    public async Task ExecuteAsync(List<FppMultiSyncSystemsResponse.FppSystem> systems, CancellationToken cancellationToken)
    {
        foreach (var system in systems)
        {
            await ExecuteAsync(system, cancellationToken);
        }
    }
}
