using Almostengr.Extensions;
using Microsoft.Extensions.Options;

namespace Almostengr.Common.TheAlmostEngineer;

public sealed class LightShowHttpClient : ILightShowHttpClient
{
    const string FPP_PHP = "fpp.php";
    private readonly HttpClient _httpClient;

    public LightShowHttpClient(IOptions<LightShowOptions> options, HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(options.Value.ApiUrl);
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("X-Auth-Token", options.Value.ApiKey);
    }

    public async Task DeleteSongsInQueueAsync(CancellationToken cancellationToken)
    {
        HttpResponseMessage response = await _httpClient.DeleteAsync(FPP_PHP, cancellationToken);
        await response.WasRequestSuccessfulAsync(cancellationToken);
    }

    public async Task<LightShowDisplayResponse> GetNextSongInQueueAsync(CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync(FPP_PHP, cancellationToken);
        await response.WasRequestSuccessfulAsync(cancellationToken);
        return await response.DeserializeResponseBodyAsync<LightShowDisplayResponse>(cancellationToken);
    }

    public async Task<LightShowDisplayResponse> PostDisplayInfoAsync(LightShowDisplayRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsync(FPP_PHP, request.SerializeRequestBodyAsync<LightShowDisplayRequest>(), cancellationToken);
        await response.WasRequestSuccessfulAsync(cancellationToken);
        return await response.DeserializeResponseBodyAsync<LightShowDisplayResponse>(cancellationToken);
    }
}

public interface ILightShowHttpClient
{
    Task<LightShowDisplayResponse> PostDisplayInfoAsync(LightShowDisplayRequest request, CancellationToken cancellationToken);
    Task<LightShowDisplayResponse> GetNextSongInQueueAsync(CancellationToken cancellationToken);
    Task DeleteSongsInQueueAsync(CancellationToken cancellationToken);
}
