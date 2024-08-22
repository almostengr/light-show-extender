using Almostengr.Common;
using Almostengr.HomeAssistant.Common;
using Almostengr.HomeAssistant.DomainService;

namespace Almostengr.HomeAssistant.Infrastructure;

public sealed class HomeAssistantHttpClient : IHomeAssistantHttpClient
{
    private readonly HttpClient _httpClient;

    public HomeAssistantHttpClient(HomeAssistantOptions options)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(options.ApiUrl);
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authentication", $"Bearer: {options.ApiKey}");
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

