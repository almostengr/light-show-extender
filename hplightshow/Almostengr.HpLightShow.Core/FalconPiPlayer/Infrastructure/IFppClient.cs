
using Almostengr.HpLightShow.Core.FalconPiPlayer.DataTransferObjects;

namespace Almostengr.HpLightShow.Core.FalconPiPlayer.Infrastructure;

public interface IFppClient
{
    Task<FppStatusDto> GetFppdStatusAsync();
    Task<string> StartPlaylistAsync(string playlist);
}