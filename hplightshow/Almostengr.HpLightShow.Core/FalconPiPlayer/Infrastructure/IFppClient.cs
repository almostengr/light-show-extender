
using Almostengr.HpLightShow.Core.FalconPiPlayer.Resources;

namespace Almostengr.HpLightShow.Core.FalconPiPlayer.Infrastructure;

public interface IFppClient
{
    Task<FppStatusResource> GetFppdStatusAsync();
    Task<string> StartPlaylistAsync(string playlist);
}