using Almostengr.Common.Utilities;

namespace Almostengr.Common.HomeAssistant;

public sealed class TurnOffSwitchRequest : BaseSwitchRequest
{
    public TurnOffSwitchRequest(string entityId) : base(entityId)
    {
    }
}

public sealed class TurnOffSwitchResponse  : BaseResponse
{
}
