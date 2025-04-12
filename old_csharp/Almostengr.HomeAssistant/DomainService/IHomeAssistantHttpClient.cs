namespace Almostengr.HomeAssistant.DomainService;

public interface IHomeAssistantHttpClient
{
    Task<TurnOffSwitchResponse> TurnOffSwitchAsync(CancellationToken cancellationToken, TurnOffSwitchRequest request);
    Task<TurnOnSwitchResponse> TurnOnSwitchAsync(CancellationToken cancellationToken, TurnOnSwitchRequest request);
}