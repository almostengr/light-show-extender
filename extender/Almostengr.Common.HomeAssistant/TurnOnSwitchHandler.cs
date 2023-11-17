using Almostengr.Extensions;
using Almostengr.Common.HomeAssistant.Common;
using Almostengr.Extensions.Logging;

namespace Almostengr.Common.HomeAssistant;

public class TurnOnSwitchHandler : IQueryHandler<TurnOnSwitchRequest, TurnOnSwitchResponse>
{
    private readonly IHomeAssistantHttpClient _homeAssistantHttpClient;
    private readonly ILoggingService<TurnOnSwitchHandler> _loggingService;

    public TurnOnSwitchHandler(
        IHomeAssistantHttpClient homeAssistantHttpClient,
        ILoggingService<TurnOnSwitchHandler> loggingService)
    {
        _loggingService = loggingService;
        _homeAssistantHttpClient = homeAssistantHttpClient;
    }

    public async Task<TurnOnSwitchResponse> ExecuteAsync(TurnOnSwitchRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await _homeAssistantHttpClient.TurnOnSwitchAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            _loggingService.Error(ex, ex.Message);
            return null;
        }
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
