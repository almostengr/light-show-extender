using Almostengr.Common.Utilities;

namespace Almostengr.Common.HomeAssistant;

public sealed class TurnOnSwitchRequest : BaseSwitchRequest
{
    public TurnOnSwitchRequest(string entityId) : base(entityId)
    {
    }
}

public sealed class TurnOnSwitchResult : BaseResultDto
{
}
