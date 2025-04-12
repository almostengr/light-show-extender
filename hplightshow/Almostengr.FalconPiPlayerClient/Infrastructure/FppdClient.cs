using Almostengr.FalconPiPlayerClient.DomainServices.Resources;
using Almostengr.FalconPiPlayerClient.DomainServices.Interfaces;

namespace Almostengr.FalconPiPlayerClient.Infrastructure;

public sealed class FppdClient : BaseClient, IFppdHttpClient
{
    public FppdClient(HttpClient httpClient) : base(httpClient) { }

    public async Task<FppdE131StatsResource> GetE131StatsAsync()
    {
        var response = await _httpClient.GetAsync("api/fppd/e131stats");
        var result = await DeserializeResponseBodyAsync<FppdE131StatsResource>(response);
        return result;
    }

    public async Task<FppdEffectsResource> GetEffectsAsync()
    {
        var response = await _httpClient.GetAsync("api/fppd/effects");
        var result = await DeserializeResponseBodyAsync<FppdEffectsResource>(response);
        return result;
    }

    public async Task<FppLogResource> GetLogAsync()
    {
        var response = await _httpClient.GetAsync("api/fppd/log");
        var result = await DeserializeResponseBodyAsync<FppLogResource>(response);
        return result;
    }

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

    public async Task<FppScheduleResource> GetScheduleAsync()
    {
        var response = await _httpClient.GetAsync("api/fppd/schedule");
        var result = await DeserializeResponseBodyAsync<FppScheduleResource>(response);
        return result;
    }

    public async Task<FppdStatusResource> GetStatusAsync()
    {
        var response = await _httpClient.GetAsync("api/fppd/status");
        var result = await DeserializeResponseBodyAsync<FppdStatusResource>(response);
        return result;
    }

    public async Task<FppdTestingResource> GetTestingAsync()
    {
        var response = await _httpClient.GetAsync("api/fppd/testing");
        var result = await DeserializeResponseBodyAsync<FppdTestingResource>(response);
        return result;
    }

    public async Task<FppVersionResource> GetVersionAsync()
    {
        var response = await _httpClient.GetAsync("api/fppd/version");
        var result = await DeserializeResponseBodyAsync<FppVersionResource>(response);
        return result;
    }

    public async Task<FppVolumeResource> GetVolumeAsync()
    {
        var response = await _httpClient.GetAsync("api/fppd/volume");
        var result = await DeserializeResponseBodyAsync<FppVolumeResource>(response);
        return result;
    }

    public Task<FppMultiSyncSystemsResource> MultiSyncSystemsResource()
    {
        throw new NotImplementedException();
    }

    public Task StartPlaylistAsync(string sequenceOverride)
    {
        throw new NotImplementedException();
    }
}
