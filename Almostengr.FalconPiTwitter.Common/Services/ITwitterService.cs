namespace Almostengr.FalconPiTwitter.Common.Services
{
    public interface ITwitterService
    {
        string GetRandomChristmasHashTags();
        string GetRandomNewYearHashTags();
        Task PostTweetAlarmAsync(string alarmMessage);
        Task<bool> PostTweetAsync(string tweet);
        Task<string> PostCurrentSongAsync(string title, string artist);
    }
}