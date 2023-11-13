using Almostengr.Extensions;
using Almostengr.Common.HomeAssistant.Common;

namespace Almostengr.Common.HomeAssistant;

public static class TurnOffSwitchHandler
{
    public static async Task<TurnOffSwitchResponse> Handle(IHomeAssistantHttpClient homeAssistantHttpClient, string entityId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(entityId))
        {
            throw new ArgumentNullException(nameof(entityId));
        }

        var request = new TurnOffSwitchRequest(entityId);
        return await homeAssistantHttpClient.TurnOffSwitchAsync(request, cancellationToken);
    }
}

public sealed class TurnOffSwitchRequest : BaseSwitchRequest
{
    public TurnOffSwitchRequest(string entityId) : base(entityId)
    {
    }
}

public sealed class TurnOffSwitchResponse  : BaseResponse
{
}
