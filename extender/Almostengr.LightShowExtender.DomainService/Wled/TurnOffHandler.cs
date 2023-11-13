using Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

namespace Almostengr.LightShowExtender.DomainService.Wled;

public static class TurnOffHandler
{
    public static async Task<WledJsonResponse> Handle(IWledHttpClient wledHttpClient, FppMultiSyncSystemsResponse.FppSystem system, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(system.Address))
        {
            throw new ArgumentNullException(nameof(system));
        }

        var request = new WledJsonStateRequest(false);
        return await wledHttpClient.PostStateAsync(system.Address, request, cancellationToken);
    }

    public static async Task Handle(IWledHttpClient wledHttpClient, List<FppMultiSyncSystemsResponse.FppSystem> systems, CancellationToken cancellationToken)
    {
        foreach(var system in systems)
        {
            await Handle(wledHttpClient, system, cancellationToken);
        }
    }
}
