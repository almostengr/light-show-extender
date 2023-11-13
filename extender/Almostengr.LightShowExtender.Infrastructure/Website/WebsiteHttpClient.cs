using Almostengr.Extensions;
using Almostengr.LightShowExtender.DomainService.Website;
using Almostengr.LightShowExtender.DomainService.Website.Common;
using Microsoft.Extensions.Options;

namespace Almostengr.LightShowExtender.Infrastructure.Website;

public sealed class WebsiteHttpClient : IWebsiteHttpClient
{
    const string BACKEND_ROUTE = "api.php";
    private readonly HttpClient _httpClient;

    public WebsiteHttpClient(IOptions<WebsiteOptions> options)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(options.Value.ApiUrl);
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("X-Auth-Token", options.Value.ApiKey);
    }

    public async Task DeleteSongsInQueueAsync(CancellationToken cancellationToken)
    {
        await _httpClient.DeleteAsync(BACKEND_ROUTE, cancellationToken);
    }

    public async Task<LightShowDisplayResponse> GetNextSongInQueueAsync(CancellationToken cancellationToken)
    {
        return await _httpClient.GetAsync<LightShowDisplayResponse>(BACKEND_ROUTE, cancellationToken);
    }

    public async Task<LightShowDisplayResponse> PostDisplayInfoAsync(WebsiteDisplayInfoRequest request, CancellationToken cancellationToken)
    {
        return await _httpClient.PostAsync<WebsiteDisplayInfoRequest, LightShowDisplayResponse>(BACKEND_ROUTE, request, cancellationToken);
    }
}
