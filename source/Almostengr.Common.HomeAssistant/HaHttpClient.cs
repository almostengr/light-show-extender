using Almostengr.Common.Utilities;

namespace Almostengr.Common.HomeAssistant;

public sealed class HaHttpClient : BaseHttpClient, IHaHttpClient
{
    public HaHttpClient(string baseAddress, string apiKey)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(baseAddress);
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authentication", $"Bearer: {apiKey}");
    }

    public async Task<TurnOnSwitchResult> TurnOnSwitchAsync(TurnOnSwitchRequest request, CancellationToken cancellationToken)
    {
        string route = "api/services/switch/turn_on";
        var result = await HttpPostAsync<TurnOnSwitchRequest, TurnOnSwitchResult>(route, request, cancellationToken);
        return result;
    }

    public async Task<TurnOffSwitchResult> TurnOffSwitchAsync(TurnOffSwitchRequest request, CancellationToken cancellationToken)
    {
        string route = "api/services/switch/turn_off";
        var result = await HttpPostAsync<TurnOffSwitchRequest, TurnOffSwitchResult>(route, request, cancellationToken);
        return result;
    }
}

public interface IHaHttpClient : IBaseHttpClient
{
    Task<TurnOffSwitchResult> TurnOffSwitchAsync(TurnOffSwitchRequest request, CancellationToken cancellationToken);
    Task<TurnOnSwitchResult> TurnOnSwitchAsync(TurnOnSwitchRequest request, CancellationToken cancellationToken);
}