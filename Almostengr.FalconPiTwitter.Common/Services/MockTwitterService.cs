using System.Text;
using Almostengr.FalconPiTwitter.Services;
using Microsoft.Extensions.Logging;

namespace Almostengr.FalconPiTwitter.Common.Services
{
    public class MockTwitterService : ITwitterService
    {
        private readonly ILogger<MockTwitterService> _logger;

        public MockTwitterService(ILogger<MockTwitterService> logger)
        {
            _logger = logger;
        }

        public string GetRandomChristmasHashTags()
        {
            return "#MerryChristmas";
        }

        public string GetRandomNewYearHashTags()
        {
            return "#HappyNewYear";
        }

        public async Task<string> PostCurrentSongAsync(string title, string artist, string playlist)
        {
            StringBuilder sb = new();
            sb.Append(title + " " + artist);
            await PostTweetAsync(sb.ToString());
            return title;
        }

        public async Task PostTweetAlarmAsync(string alarmMessage)
        {
            _logger.LogWarning(alarmMessage);
            await Task.Delay(TimeSpan.FromSeconds(1));
        }

        public async Task<bool> PostTweetAsync(string tweet)
        {
            _logger.LogInformation(tweet);
            await Task.Delay(TimeSpan.FromSeconds(1));
            return true;
        }
    }
}