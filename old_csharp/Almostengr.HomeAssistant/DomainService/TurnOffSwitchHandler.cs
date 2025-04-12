using Almostengr.Common.Query;

namespace Almostengr.HomeAssistant.DomainService;

public class TurnOffSwitchHandler : IQueryHandler<TurnOffSwitchRequest, TurnOffSwitchResponse>
{
    private readonly IHomeAssistantHttpClient _homeAssistantHttpClient;

    public TurnOffSwitchHandler(IHomeAssistantHttpClient homeAssistantHttpClient)
    {
        _homeAssistantHttpClient = homeAssistantHttpClient;
    }

    public async Task<TurnOffSwitchResponse> ExecuteAsync(CancellationToken cancellationToken, TurnOffSwitchRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (string.IsNullOrWhiteSpace(request.EntityId))
        {
            throw new ArgumentNullException(nameof(request.EntityId));
        }

        return await _homeAssistantHttpClient.TurnOffSwitchAsync(cancellationToken, request);
    }
}
