namespace Almostengr.HomeAssistant.DomainService;

public interface IHomeAssistantHttpClient
{
    Task<TurnOffSwitchResponse> TurnOffSwitchAsync(TurnOffSwitchRequest request, CancellationToken cancellationToken);
    Task<TurnOnSwitchResponse> TurnOnSwitchAsync(TurnOnSwitchRequest request, CancellationToken cancellationToken);
}