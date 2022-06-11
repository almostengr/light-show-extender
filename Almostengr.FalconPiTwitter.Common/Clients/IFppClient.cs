using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.DataTransferObjects;

namespace Almostengr.FalconPiTwitter.Clients
{
    public interface IFppClient
    {
        Task<FalconFppdStatusDto> GetFppdStatusAsync(string address);
        Task<FalconMediaMetaDto> GetCurrentSongMetaDataAsync(string current_Song);
        Task<FalconFppdMultiSyncSystemsDto> GetMultiSyncStatusAsync(string address);
    }
}