using Almostengr.Common.Utilities;

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

    public async Task<HaSwitchResponseDto> TurnOnSwitchAsync(HaSwitchRequestDto requestDto)
    {
        string route = "api/services/switch/turn_on";
        StringContent content = HttpClientUtilities.SerializeRequestBodyAsync<HaSwitchRequestDto>(requestDto);
        HttpResponseMessage response = await _httpClient.PostAsync(route, content);
        response.EnsureSuccessStatusCode();
        return await HttpClientUtilities.DeserializeResponseBodyAsync<HaSwitchResponseDto>(response);
    }

    public async Task<HaSwitchResponseDto> TurnOffSwitchAsync(HaSwitchRequestDto requestDto)
    {
        string route = "api/services/switch/turn_off";
        StringContent content = HttpClientUtilities.SerializeRequestBodyAsync<HaSwitchRequestDto>(requestDto);
        HttpResponseMessage response = await _httpClient.PostAsync(route, content);
        response.EnsureSuccessStatusCode();
        return await HttpClientUtilities.DeserializeResponseBodyAsync<HaSwitchResponseDto>(response);
    }
}