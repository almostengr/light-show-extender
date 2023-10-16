using Almostengr.Common.Utilities;

namespace Almostengr.Common.TheAlmostEngineer;

public sealed class EngineerHttpClient :IEngineerHttpClient
{
    private readonly HttpClient _httpClient;
    private const string FPP_API_ROUTE = "fpp.php";

    public EngineerHttpClient(Uri apiUri, string apiKey)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = apiUri;
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("X-Auth-Token", apiKey);
    }

    public async Task DeleteAllSongsInQueueAsync()
    {
        var response = await _httpClient.DeleteAsync(FPP_API_ROUTE);
        response.EnsureSuccessStatusCode();
    }

    public async Task<EngineerResponseDto> GetFirstUnplayedRequestAsync()
    {
        var response = await _httpClient.GetAsync(FPP_API_ROUTE);
        response.EnsureSuccessStatusCode();
        return await HttpClientUtilities.DeserializeResponseBodyAsync<EngineerResponseDto>(response);
    }

    public async Task<EngineerResponseDto> PostDisplayInfoAsync(EngineerDisplayRequestDto requestDto)
    {
        StringContent content = HttpClientUtilities.SerializeRequestBodyAsync<EngineerDisplayRequestDto>(requestDto);
        HttpResponseMessage response = await _httpClient.PostAsync(FPP_API_ROUTE, content);
        response.EnsureSuccessStatusCode();
        return await HttpClientUtilities.DeserializeResponseBodyAsync<EngineerResponseDto>(response);
    }
}