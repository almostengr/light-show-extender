using Almostengr.FalconPiPlayerClient.DomainServices.Resources;

namespace Almostengr.FalconPiPlayerClient.DomainServices.Interfaces;

public interface IFppdHttpClient
{
    public Task<FppdStatusResource> GetStatusAsync();
    Task<FppMultiSyncSystemsResource> MultiSyncSystemsResource();
    Task StartPlaylistAsync(string sequenceOverride);
}