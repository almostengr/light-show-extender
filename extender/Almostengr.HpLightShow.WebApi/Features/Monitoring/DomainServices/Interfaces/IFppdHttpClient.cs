using Almostengr.HpLightShow.WebApi.Features.Monitoring.DomainServices.Resources;

namespace Almostengr.HpLightShow.WebApi.Features.Monitoring.DomainServices.Interfaces;

public interface IFppdHttpClient
{
    Task<FppMultiSyncStatusResource> GetMultiSyncStatusAsync();
    Task<FppMultiSyncSystemsResource> GetMultiSyncSystemsAsync();
    Task<FppdStatusResource> GetStatusAsync();
    Task<FppVolumeResource> GetVolumeAsync();
    Task<string> StartPlaylistAsync(string sequence);
    Task<List<string>> GetMediaAsync();
    Task<MediaMetaResource> GetMediaMetaAsync(string mediaName);
}