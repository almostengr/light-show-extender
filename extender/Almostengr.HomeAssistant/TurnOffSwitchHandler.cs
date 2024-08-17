using Almostengr.Extensions;
using Almostengr.Common.HomeAssistant.Common;
using Almostengr.Extensions.Logging;

namespace Almostengr.Common.HomeAssistant;

public class TurnOffSwitchHandler : IQueryHandler<TurnOffSwitchRequest, TurnOffSwitchResponse>
{
    private readonly IHomeAssistantHttpClient _homeAssistantHttpClient;
    private readonly ILoggingService<TurnOffSwitchHandler> _loggingService;

    public TurnOffSwitchHandler(
        IHomeAssistantHttpClient homeAssistantHttpClient,
        ILoggingService<TurnOffSwitchHandler> loggingService)
    {
        _homeAssistantHttpClient = homeAssistantHttpClient;
        _loggingService = loggingService;
    }

    public async Task<TurnOffSwitchResponse> ExecuteAsync(TurnOffSwitchRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrWhiteSpace(request.EntityId))
            {
                throw new ArgumentNullException(nameof(request.EntityId));
            }
            
            return await _homeAssistantHttpClient.TurnOffSwitchAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            _loggingService.Error(ex, ex.Message);
            return null!;
        }
    }
}

public sealed class TurnOffSwitchRequest : BaseSwitchRequest
{
    public TurnOffSwitchRequest(string entityId) : base(entityId)
    {
    }
}

public sealed class TurnOffSwitchResponse : IQueryResponse
{
}
