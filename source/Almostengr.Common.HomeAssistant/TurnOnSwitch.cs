using Almostengr.Common.Utilities;

namespace Almostengr.Common.HomeAssistant;

public sealed class TurnOnSwitchCommand : BaseSwitchCommand
{
    public TurnOnSwitchCommand(string entityId) : base(entityId)
    {
    }
}

public sealed class TurnOnSwitchResult
{
}

public sealed class TurnOnSwitchCommandHandler
{
    private IHomeAssistantHttpClient _client;

    public TurnOnSwitchCommandHandler(IHomeAssistantHttpClient client)
    {
        _client = client;
    }

    public async Task<TurnOnSwitchResult> HandleAsync(TurnOnSwitchCommand request, CancellationToken cancellationToken)
    {
        StringContent content = HttpClientUtilities.SerializeRequestBodyAsync<TurnOnSwitchCommand>(request);
        var response = await _client.TurnOnSwitchAsync(content, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await HttpClientUtilities.DeserializeResponseBodyAsync<TurnOnSwitchResult>(response);
    }
}