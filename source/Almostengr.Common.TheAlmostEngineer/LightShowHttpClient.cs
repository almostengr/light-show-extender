using Almostengr.HttpClient;
using Microsoft.Extensions.Options;

namespace Almostengr.Common.TheAlmostEngineer;

public sealed class LightShowHttpClient : BaseHttpClient, ILightShowHttpClient
{
    const string FPP_PHP = "fpp.php";

    public LightShowHttpClient(IOptions<LightShowOptions> options)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(options.Value.ApiUrl);
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("X-Auth-Token", options.Value.ApiKey);
    }

    public async Task DeleteSongsInQueueAsync(CancellationToken cancellationToken)
    {
        await HttpDeleteAsync<string>(FPP_PHP, cancellationToken);
    }

    public async Task<LightShowDisplayResponse> GetNextSongInQueueAsync(CancellationToken cancellationToken)
    {
        var result = await HttpGetAsync<LightShowDisplayResponse>(FPP_PHP, cancellationToken);
        return result;
    }

    public async Task<LightShowDisplayResponse> PostDisplayInfoAsync(LightShowDisplayRequest request, CancellationToken cancellationToken)
    {
        var result = await HttpPostAsync<LightShowDisplayRequest, LightShowDisplayResponse>(FPP_PHP, request, cancellationToken);
        return result;
    }
}

public interface ILightShowHttpClient
{
    Task<LightShowDisplayResponse> PostDisplayInfoAsync(LightShowDisplayRequest request, CancellationToken cancellationToken);
    Task<LightShowDisplayResponse> GetNextSongInQueueAsync(CancellationToken cancellationToken);
    Task DeleteSongsInQueueAsync(CancellationToken cancellationToken);
}
