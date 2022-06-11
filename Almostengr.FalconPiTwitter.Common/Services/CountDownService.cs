using Almostengr.FalconPiTwitter.Common.Extensions;
using Almostengr.FalconPiTwitter.DataTransferObjects;
using Almostengr.FalconPiTwitter.Services;
using Microsoft.Extensions.Logging;

namespace Almostengr.FalconPiTwitter.Common.Services
{
    public class CountDownService : ICountDownService
    {
        private readonly ITwitterService _twitterService;
        private readonly IFppService _fppService;
        private readonly AppSettings _appSettings;
        private readonly ILogger<CountDownService> _logger;

        public CountDownService(ITwitterService twitterService, IFppService fppService,
            AppSettings appSettings, ILogger<CountDownService> logger)
        {
            _twitterService = twitterService;
            _fppService = fppService;
            _appSettings = appSettings;
            _logger = logger;
        }

        public async Task TimeUntilNextLightShowAsync()
        {
            FalconFppdStatusDto fppStatus = await _fppService.GetFppdStatusAsync(_appSettings.FppHosts[0]);

            if (fppStatus == null)
            {
                string message = "FPP did not provide a status";
                _logger.LogError(message);
                throw new ArgumentNullException(message);
            }
            
            if (fppStatus.Next_Playlist.Playlist.ContainsOfflineTestOrNull())
            {
                await Task.CompletedTask;
            }

            string nextPlaylistDateTime = fppStatus.Next_Playlist.Start_Time
                .Substring(0, fppStatus.Next_Playlist.Start_Time.IndexOf(" - "))
                .Replace(" @ ", "T");
            // nextPlaylistDateTime = nextPlaylistDateTime.Replace(" @ ", "T");

            if (DateTime.TryParse(nextPlaylistDateTime, out DateTime showStartDateTime))
            {
                _logger.LogError("Invalid date time or did not match expected format");
            }

            await _twitterService.PostTweetAsync(
                $"{CalculateTimeBetween(DateTime.Now, showStartDateTime)} until the next Light Show. ");
        }

        public async Task TimeUntilChristmasAsync()
        {
            DateTime christmasDateTime = new DateTime(DateTime.Now.Year, 12, 25, 0, 0, 0);
            DateTime currentDateTime = DateTime.Now;

            if (currentDateTime.Date == christmasDateTime.Date)
            {
                await _twitterService.PostTweetAsync("Today is Christmas!");
            }

            if (currentDateTime < christmasDateTime)
            {
                await _twitterService.PostTweetAsync(
                    $"{CalculateTimeBetween(currentDateTime, christmasDateTime)} until Christmas. ");
            }
        }

        private string CalculateTimeBetween(DateTime startDate, DateTime endDate)
        {
            TimeSpan timeDiff = endDate - startDate;

            string output = string.Empty;
            output += (timeDiff.Days > 0 ? (timeDiff.Days + (timeDiff.Days == 1 ? " day " : " days ")) : string.Empty);
            output += (timeDiff.Hours > 0 ? (timeDiff.Hours + (timeDiff.Hours == 1 ? " hour " : " hours ")) : string.Empty);
            output += (timeDiff.Minutes > 0 ? (timeDiff.Minutes + (timeDiff.Minutes == 1 ? " minute " : " minutes ")) : string.Empty);

            return output;
        }

    }
}