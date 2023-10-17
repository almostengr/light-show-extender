namespace Almostengr.Common.HomeAssistant;

public sealed class HomeAssistantHttpClient : IHomeAssistantHttpClient
{
    private readonly HttpClient _httpClient;

    public HomeAssistantHttpClient(string hostAddress, string apiKey)
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization:", $"Bearer {apiKey}");
        _httpClient.BaseAddress = new Uri(hostAddress);
    }

    public async Task<HttpResponseMessage> TurnOnSwitchAsync(HttpContent content, CancellationToken cancellationToken)
    {
        string route = "api/services/switch/turn_on";
        return await _httpClient.PostAsync(route, content, cancellationToken);
    }

    public async Task<HttpResponseMessage> TurnOffSwitchAsync(HttpContent content, CancellationToken cancellationToken)
    {
        string route = "api/services/switch/turn_off";
        return await _httpClient.PostAsync(route, content, cancellationToken);
    }
}

public interface IHomeAssistantHttpClient
{
    Task<HttpResponseMessage> TurnOffSwitchAsync(HttpContent content, CancellationToken cancellationToken);
    Task<HttpResponseMessage> TurnOnSwitchAsync(HttpContent content, CancellationToken cancellationToken);
}