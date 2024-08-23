using Almostengr.Common.Query;

namespace Almostengr.HomeAssistant.DomainService;

public class TurnOnSwitchHandler : IQueryHandler<TurnOnSwitchRequest, TurnOnSwitchResponse>
{
    private readonly IHomeAssistantHttpClient _homeAssistantHttpClient;

    public TurnOnSwitchHandler(IHomeAssistantHttpClient homeAssistantHttpClient)
    {
        _homeAssistantHttpClient = homeAssistantHttpClient;
    }

    public async Task<TurnOnSwitchResponse> ExecuteAsync(CancellationToken cancellationToken, TurnOnSwitchRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (string.IsNullOrWhiteSpace(request.EntityId))
        {
            throw new ArgumentNullException(nameof(request.EntityId));
        }

        return await _homeAssistantHttpClient.TurnOnSwitchAsync(cancellationToken, request);
    }
}
