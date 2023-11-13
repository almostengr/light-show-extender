using Almostengr.Extensions;
using Almostengr.Common.HomeAssistant.Common;

namespace Almostengr.Common.HomeAssistant;

public static class TurnOnSwitchHandler
{
    public static async Task<TurnOnSwitchResponse> Handle(IHomeAssistantHttpClient homeAssistantHttpClient, string entityId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(entityId))
        {
            throw new ArgumentNullException(nameof(entityId));
        }

        var request = new TurnOnSwitchRequest(entityId);
        return await homeAssistantHttpClient.TurnOnSwitchAsync(request, cancellationToken);
    }
}

public sealed class TurnOnSwitchRequest : BaseSwitchRequest
{
    public TurnOnSwitchRequest(string entityId) : base(entityId)
    {
    }
}

public sealed class TurnOnSwitchResponse : BaseResponse
{
}
