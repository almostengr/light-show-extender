using Almostengr.HpLightShow.WebApi.Features.Common.Infrastructure;
using Almostengr.HpLightShow.WebApi.Features.Monitoring.DomainServices.Interfaces;
using Almostengr.HpLightShow.WebApi.Features.Monitoring.DomainServices.Resources;

namespace Almostengr.HpLightShow.WebApi.Features.Monitoring.Infrastructure;

public sealed class FppdClient : BaseClient, IFppdHttpClient
{
    public FppdClient(HttpClient httpClient) : base(httpClient) { }

    public async Task<FppMultiSyncStatusResource> GetMultiSyncStatusAsync()
    {
        var response = await _httpClient.GetAsync("api/fppd/multiSyncStats");
        var result = await DeserializeResponseBodyAsync<FppMultiSyncStatusResource>(response);
        return result;
    }

    public async Task<FppMultiSyncSystemsResource> GetMultiSyncSystemsAsync()
    {
        var response = await _httpClient.GetAsync("api/fppd/multiSyncSystems");
        var result = await DeserializeResponseBodyAsync<FppMultiSyncSystemsResource>(response);
        return result;
    }

    public async Task<FppdStatusResource> GetStatusAsync()
    {
        var response = await _httpClient.GetAsync("api/fppd/status");
        var result = await DeserializeResponseBodyAsync<FppdStatusResource>(response);
        return result;
    }

    public async Task<FppVolumeResource> GetVolumeAsync()
    {
        var response = await _httpClient.GetAsync("api/fppd/volume");
        var result = await DeserializeResponseBodyAsync<FppVolumeResource>(response);
        return result;
    }

    public async Task<string> StartPlaylistAsync(string sequence)
    {
        var response = await _httpClient.GetAsync($"api/playlist/${sequence}/start");
        var result = await response.Content.ReadAsStringAsync();
        return result;
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
