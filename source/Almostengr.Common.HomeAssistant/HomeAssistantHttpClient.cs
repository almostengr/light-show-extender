using Microsoft.Extensions.Options;
using Almostengr.Common.Utilities;

namespace Almostengr.Common.HomeAssistant;

public sealed class HomeAssistantHttpClient : BaseHttpClient, IHomeAssistantHttpClient
{
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
        var result = await HttpPostAsync<TurnOnSwitchRequest, TurnOnSwitchResponse>(route, request, cancellationToken);
        return result;
    }

    public async Task<TurnOffSwitchResponse> TurnOffSwitchAsync(TurnOffSwitchRequest request, CancellationToken cancellationToken)
    {
        string route = "api/services/switch/turn_off";
        var result = await HttpPostAsync<TurnOffSwitchRequest, TurnOffSwitchResponse>(route, request, cancellationToken);
        return result;
    }
}

public interface IHomeAssistantHttpClient : IBaseHttpClient
{
    Task<TurnOffSwitchResponse> TurnOffSwitchAsync(TurnOffSwitchRequest request, CancellationToken cancellationToken);
    Task<TurnOnSwitchResponse> TurnOnSwitchAsync(TurnOnSwitchRequest request, CancellationToken cancellationToken);
}
