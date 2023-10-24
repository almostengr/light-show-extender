using Almostengr.Extensions;
using Microsoft.Extensions.Options;

namespace Almostengr.Common.TheAlmostEngineer;

public sealed class LightShowHttpClient : ILightShowHttpClient
{
    const string FPP_PHP = "fpp.php";
    private readonly HttpClient _httpClient;

    public LightShowHttpClient(IOptions<LightShowOptions> options)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(options.Value.ApiUrl);
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("X-Auth-Token", options.Value.ApiKey);
    }

    public async Task DeleteSongsInQueueAsync(CancellationToken cancellationToken)
    {
        await _httpClient.DeleteAsync(FPP_PHP, cancellationToken);
    }

    public async Task<LightShowDisplayResponse> GetNextSongInQueueAsync(CancellationToken cancellationToken)
    {
        return await _httpClient.GetAsync<LightShowDisplayResponse>(FPP_PHP, cancellationToken);
    }

    public async Task<LightShowDisplayResponse> PostDisplayInfoAsync(LightShowDisplayRequest request, CancellationToken cancellationToken)
    {
        return await _httpClient.PostAsync<LightShowDisplayRequest, LightShowDisplayResponse>(FPP_PHP, request, cancellationToken);
    }
}

public interface ILightShowHttpClient
{
    Task<LightShowDisplayResponse> PostDisplayInfoAsync(LightShowDisplayRequest request, CancellationToken cancellationToken);
    Task<LightShowDisplayResponse> GetNextSongInQueueAsync(CancellationToken cancellationToken);
    Task DeleteSongsInQueueAsync(CancellationToken cancellationToken);
}
