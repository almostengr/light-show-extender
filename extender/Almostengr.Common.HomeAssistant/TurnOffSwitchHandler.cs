using Almostengr.Extensions;
using Almostengr.Common.HomeAssistant.Common;
using Almostengr.Extensions.Logging;

namespace Almostengr.Common.HomeAssistant;

public class TurnOffSwitchHandler
{
    private readonly IHomeAssistantHttpClient _homeAssistantHttpClient;
    private readonly ILoggingService<TurnOffSwitchHandler> _loggingService;

    public TurnOffSwitchHandler(IHomeAssistantHttpClient homeAssistantHttpClient, ILoggingService<TurnOffSwitchHandler> loggingService)
    {
        _homeAssistantHttpClient = homeAssistantHttpClient;
        _loggingService = loggingService;
    }

    public async Task<TurnOffSwitchResponse> HandleAsync(TurnOffSwitchRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await homeAssistantHttpClient.TurnOffSwitchAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            _loggingService.Error(ex.Message);
            return null;
        }
    }
}

public sealed class TurnOffSwitchRequest : BaseSwitchRequest
{
    public TurnOffSwitchRequest(string entityId) : base(entityId)
    {
    }
}

public sealed class TurnOffSwitchResponse : BaseResponse
{
}
