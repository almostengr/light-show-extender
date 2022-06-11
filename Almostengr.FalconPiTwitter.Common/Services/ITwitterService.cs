namespace Almostengr.FalconPiTwitter.Services
{
    public interface ITwitterService
    {
        string GetRandomChristmasHashTag();
        string GetRandomNewYearHashTag();
        Task PostTweetAlarmAsync(string alarmMessage);
        Task<bool> PostTweetAsync(string tweet);
        Task<string> PostCurrentSongAsync(string previousSong, string title, string artist, string playlist);
    }
}