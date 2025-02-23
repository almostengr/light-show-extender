using Almostengr.FalconPiPlayerClient.DomainServices.Interfaces;
using Almostengr.FalconPiPlayerClient.DomainServices.Resources;

namespace Almostengr.FalconPiPlayerClient.Infrastructure;

public sealed class MediaHttpClient : BaseClient, IMediaHttpClient
{
    public MediaHttpClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<List<string>> GetMediaAsync()
    {
        var response = await _httpClient.GetAsync("api/media");
        var result = await DeserializeResponseBodyAsync<List<string>>(response);
        return result;
    }

    public async Task<MediaMetaResource> GetMediaMetaAsync(string mediaName)
    {
        string route = $"api/media/{mediaName}/meta";
        var response = await _httpClient.GetAsync(route);
        var result = await DeserializeResponseBodyAsync<MediaMetaResource>(response);
        return result;
    }
}

public sealed class MediaMetaResource : BaseResource
{
}