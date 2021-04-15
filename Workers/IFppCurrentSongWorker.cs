using System.Threading.Tasks;
using Almostengr.FalconPiMonitor.Models;

namespace Almostengr.FalconPiTwitter.Workers
{
    public interface IFppCurrentSongWorker
    {
        Task<string> PostCurrentSong(string previousTitle, string currentTitle, string artist, string album, string playlist);
        Task<FalconMediaMeta> GetCurrentSongMetaData(string currentSong);
        Task<FalconFppdStatus> GetCurrentStatus();
        Task GetTwitterUsername();
        string GetSongTitle(string notFileTitle, string tagTitle);
    }
}