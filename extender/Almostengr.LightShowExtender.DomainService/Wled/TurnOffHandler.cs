using Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

namespace Almostengr.LightShowExtender.DomainService.Wled;

public sealed class TurnOffHandler
{
    private readonly IWledHttpClient _wledHttpClient;

    public TurnOffHandler(IWledHttpClient wledHttpClient)
    {
        _wledHttpClient = wledHttpClient;
    }

    public  async Task<WledJsonResponse> Handle(FppMultiSyncSystemsResponse.FppSystem system, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(system.Address))
        {
            throw new ArgumentNullException(nameof(system));
        }

        var request = new WledJsonStateRequest(false);
        return await _wledHttpClient.PostStateAsync(system.Address, request, cancellationToken);
    }

    public  async Task Handle(List<FppMultiSyncSystemsResponse.FppSystem> systems, CancellationToken cancellationToken)
    {
        foreach(var system in systems)
        {
            await Handle(_wledHttpClient, system, cancellationToken);
        }
    }
}
