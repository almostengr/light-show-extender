using Almostengr.Common.Infrastructure;
using Almostengr.HpLightShow.WebApi.Features.Monitoring.DomainServices.Interfaces;
using Almostengr.HpLightShow.WebApi.Features.Monitoring.DomainServices.Resources;

namespace Almostengr.HpLightShow.WebApi.Features.Monitoring.Infrastructure;

public sealed class FppdClient : IFppdHttpClient
{
    private readonly HttpClient _httpClient;

    public FppdClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<FppMultiSyncStatusResource> GetMultiSyncStatusAsync()
    {
        var response = await _httpClient.GetAsync("api/fppd/multiSyncStats");
        var result = await response.DeserializeResponseBodyAsync<FppMultiSyncStatusResource>();
        return result;
    }

    public async Task<FppMultiSyncSystemsResource> GetMultiSyncSystemsAsync()
    {
        var response = await _httpClient.GetAsync("api/fppd/multiSyncSystems");
        var result = await response.DeserializeResponseBodyAsync<FppMultiSyncSystemsResource>();
        return result;
    }

    public async Task<FppdStatusResource> GetStatusAsync()
    {
        var response = await _httpClient.GetAsync("api/fppd/status");
        var result = await response.DeserializeResponseBodyAsync<FppdStatusResource>();
        return result;
    }

    public async Task<FppVolumeResource> GetVolumeAsync()
    {
        var response = await _httpClient.GetAsync("api/fppd/volume");
        var result = await response.DeserializeResponseBodyAsync<FppVolumeResource>();
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
        var result = await response.DeserializeResponseBodyAsync<List<string>>();
        return result;
    }

    public async Task<MediaMetaResource> GetMediaMetaAsync(string mediaName)
    {
        string route = $"api/media/{mediaName}/meta";
        var response = await _httpClient.GetAsync(route);
        var result = await response.DeserializeResponseBodyAsync<MediaMetaResource>();
        return result;
    }
}
