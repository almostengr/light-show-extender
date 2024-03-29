using Microsoft.Extensions.Options;
using Almostengr.Extensions;

namespace Almostengr.Common.HomeAssistant.Common;

public sealed class HomeAssistantHttpClient : IHomeAssistantHttpClient
{
    private readonly HttpClient _httpClient;

    public HomeAssistantHttpClient(IOptions<HomeAssistantOptions> options)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(options.Value.ApiUrl);
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authentication", $"Bearer: {options.Value.ApiKey}");
    }

    public async Task<TurnOnSwitchResponse> TurnOnSwitchAsync(TurnOnSwitchRequest request, CancellationToken cancellationToken)
    {
        string route = "api/services/switch/turn_on";
        return await _httpClient.PostAsync<TurnOnSwitchRequest, TurnOnSwitchResponse>(route, request, cancellationToken);
    }

    public async Task<TurnOffSwitchResponse> TurnOffSwitchAsync(TurnOffSwitchRequest request, CancellationToken cancellationToken)
    {
        string route = "api/services/switch/turn_off";
        return await _httpClient.PostAsync<TurnOffSwitchRequest, TurnOffSwitchResponse>(route, request, cancellationToken);
    }
}

public interface IHomeAssistantHttpClient
{
    Task<TurnOffSwitchResponse> TurnOffSwitchAsync(TurnOffSwitchRequest request, CancellationToken cancellationToken);
    Task<TurnOnSwitchResponse> TurnOnSwitchAsync(TurnOnSwitchRequest request, CancellationToken cancellationToken);
}
