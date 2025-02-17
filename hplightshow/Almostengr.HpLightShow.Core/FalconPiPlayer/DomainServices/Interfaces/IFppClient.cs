namespace Almostengr.HpLightShow.Core.FalconPiPlayer.DomainServices.Infrastructure;

public interface IFppClient
{
    Task<FppStatusResource> GetFppdStatusAsync();
    Task<MultiSyncSystemsResource> MultiSyncSystemsResource();
    Task<string> StartPlaylistAsync(string playlist);
}