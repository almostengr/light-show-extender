using Almostengr.Extensions;
using Almostengr.Extensions.Logging;
using Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

namespace Almostengr.LightShowExtender.DomainService.Wled;

public sealed class TurnOnWledHandler: IQueryHandler<FppMultiSyncSystemsResponse.FppSystem, WledJsonStateResponse>
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

    public async Task<WledJsonStateResponse> ExecuteAsync(FppMultiSyncSystemsResponse.FppSystem system, CancellationToken cancellationToken)
    {
        WledJsonStateResponse result;

        try
        {
            if (string.IsNullOrWhiteSpace(system.Address))
            {
                throw new ArgumentNullException(nameof(system));
            }

            var request = new WledJsonStateRequest(true);
            result =  await _wledHttpClient.PostStateAsync(system.Address, request, cancellationToken);

            if (result.Success == false)
            {
                throw new Exception($"Turn off WLED request failed {system.Address}");
            }
        }
        catch (Exception ex)
        {
            _loggingService.Error(ex.Message);
            result = null!;
        }

        return result;
    }

    public async Task ExecuteAsync(List<FppMultiSyncSystemsResponse.FppSystem> systems, CancellationToken cancellationToken)
    {
        foreach (var system in systems)
        {
            await ExecuteAsync(system, cancellationToken);
        }
    }
}
