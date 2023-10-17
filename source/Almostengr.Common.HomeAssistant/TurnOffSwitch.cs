using Almostengr.Common.Utilities;

namespace Almostengr.Common.HomeAssistant;

public sealed class TurnOffSwitchCommand : BaseSwitchCommand
{
    public TurnOffSwitchCommand(string entityId) : base(entityId)
    {
    }
}

public sealed class TurnOffSwitchResult
{
}

public sealed class TurnOffSwitchCommandHandler
{
    private IHomeAssistantHttpClient _client;

    public TurnOffSwitchCommandHandler(IHomeAssistantHttpClient client)
    {
        _client = client;
    }

    public async Task<TurnOffSwitchResult> HandleAsync(TurnOffSwitchCommand request, CancellationToken cancellationToken)
    {
        StringContent content = HttpClientUtilities.SerializeRequestBodyAsync<TurnOffSwitchCommand>(request);
        var response = await _client.TurnOffSwitchAsync(content, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await HttpClientUtilities.DeserializeResponseBodyAsync<TurnOffSwitchResult>(response);
    }
}