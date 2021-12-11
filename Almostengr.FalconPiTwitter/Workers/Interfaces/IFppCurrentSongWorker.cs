using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.DataTransferObjects;

namespace Almostengr.FalconPiTwitter.Workers
{
    public interface IFppCurrentSongWorker : IBaseWorker
    {
        Task<string> PostCurrentSongAsync(string previousTitle, string currentTitle, string artist, string album, string playlist);
        Task<FalconMediaMetaDto> GetCurrentSongMetaDataAsync(string currentSong);
        string GetSongTitle(string notFileTitle, string tagTitle);
    }
}