namespace Almostengr.Common.HomeAssistant;

public sealed class HomeAssistantService : IHomeAssistantService
{
    private readonly IHomeAssistantHttpClient _httpClient;

    public HomeAssistantService(IHomeAssistantHttpClient httpClient)
    {
        _httpClient = httpClient;        
    }

    public async Task<TurnOffSwitchResponse> TurnOffSwitchAsync(string entityId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(entityId))
        {
            throw new ArgumentNullException(nameof(entityId));
        }

        var request = new TurnOffSwitchRequest(entityId);
        return await _httpClient.TurnOffSwitchAsync(request, cancellationToken);
    }

    public async Task<TurnOnSwitchResponse> TurnOnSwitchAsync(string entityId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(entityId))
        {
            throw new ArgumentNullException(nameof(entityId));
        }

        var request = new TurnOnSwitchRequest(entityId);
        return await _httpClient.TurnOnSwitchAsync(request, cancellationToken);
    }
}

public interface IHomeAssistantService
{
    Task<TurnOffSwitchResponse> TurnOffSwitchAsync(string entityId, CancellationToken cancellationToken);
    Task<TurnOnSwitchResponse> TurnOnSwitchAsync(string entityId, CancellationToken cancellationToken);
}