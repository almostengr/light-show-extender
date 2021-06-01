using System.Threading.Tasks;
using Almostengr.FalconPiMonitor.Models;

namespace Almostengr.FalconPiTwitter.Workers
{
    public interface IFppCurrentSongWorker : IBaseWorker
    {
        Task<string> PostCurrentSongAsync(string previousTitle, string currentTitle, string artist, string album, string playlist);
        Task<FalconMediaMeta> GetCurrentSongMetaDataAsync(string currentSong);
        string GetSongTitle(string notFileTitle, string tagTitle);
    }
}