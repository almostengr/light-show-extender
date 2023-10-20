using Almostengr.HttpClient;

namespace Almostengr.Common.HomeAssistant;

public sealed class TurnOnSwitchRequest : BaseSwitchRequest
{
    public TurnOnSwitchRequest(string entityId) : base(entityId)
    {
    }
}

public sealed class TurnOnSwitchResponse : BaseResponse
{
}
