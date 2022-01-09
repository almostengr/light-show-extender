using System.Threading.Tasks;

namespace Almostengr.FalconPiTwitter.Services
{
    public interface ITwitterService : IBaseService
    {
        string GetRandomChristmasHashTag();
        Task<bool> PostTweetAsync(string tweet);
        Task<string> GetAuthenticatedUserAsync();
        Task<string> PostCurrentSongAsync(string previousSong, string title, string artist, string playlist);
        Task PostTweetAlarmAsync(string alarmMessage, int alarmCount = 0);
    }
}