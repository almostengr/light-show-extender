using System.Text;
using Almostengr.FalconPiTwitter.Clients;
using Almostengr.FalconPiTwitter.Common.Extensions;
using Almostengr.FalconPiTwitter.DataTransferObjects;
using Almostengr.FalconPiTwitter.Services;
using Microsoft.Extensions.Logging;

namespace Almostengr.FalconPiTwitter.Common.Services
{
    public class CountDownService : BaseService, ICountDownService
    {
        private readonly ITwitterService _twitterService;
        private readonly IFppClient _fppClient;
        private readonly AppSettings _appSettings;
        private readonly ILogger<CountDownService> _logger;

        public CountDownService(ITwitterService twitterService, IFppClient fppClient,
            AppSettings appSettings, ILogger<CountDownService> logger)
        {
            _twitterService = twitterService;
            _fppClient = fppClient;
            _appSettings = appSettings;
            _logger = logger;
        }

        private async Task TimeUntilNextLightShowAsync()
        {
            FalconFppdStatusDto fppStatus = await _fppClient.GetFppdStatusAsync(_appSettings.FppHosts[0]);

            if (fppStatus == null)
            {
                throw new ArgumentNullException("FPP did not provide a status");
            }

            if (fppStatus.Next_Playlist.Playlist.ContainsOfflineTestOrNull())
            {
                await Task.CompletedTask;
            }

            string nextPlaylistDateTime = fppStatus.Next_Playlist.Start_Time
                .Substring(0, fppStatus.Next_Playlist.Start_Time.IndexOf(" - "))
                .Replace(" @ ", "T");

            if (DateTime.TryParse(nextPlaylistDateTime, out DateTime showStartDateTime))
            {
                _logger.LogError("Invalid date time or did not match expected format");
                await Task.CompletedTask;
            }

            await _twitterService.PostTweetAsync(
                $"{CalculateTimeBetween(DateTime.Now, showStartDateTime)} until the next Light Show. ");
        }

        private async Task TimeUntilChristmasAsync()
        {
            DateTime christmasDateTime = new DateTime(DateTime.Now.Year, 12, 25, 0, 0, 0);
            DateTime currentDateTime = DateTime.Now;
            StringBuilder sb = new StringBuilder();

            if (currentDateTime.Date == christmasDateTime.Date)
            {
                sb.Append("Today is Christmas! ");
            }
            else if (currentDateTime < christmasDateTime)
            {
                sb.Append($"{CalculateTimeBetween(currentDateTime, christmasDateTime)} until Christmas. ");
            }

            if (sb.Length > 0)
            {
                sb.Append(_twitterService.GetRandomChristmasHashTags());
                await _twitterService.PostTweetAsync(sb.ToString());
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

        public async Task ExecuteLightShowCountdownAsync(CancellationToken cancellationToken)
        {
            while (cancellationToken.IsCancellationRequested == false)
            {
                try
                {
                    await TimeUntilNextLightShowAsync();
                    await Task.Delay(TimeSpan.FromHours(base.GetRandomWaitTime()), cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
            }
        }

        public async Task ExecuteChristmasCountdownAsync(CancellationToken cancellationToken)
        {
            while (cancellationToken.IsCancellationRequested == false)
            {
                try
                {
                    await TimeUntilChristmasAsync();
                    await Task.Delay(TimeSpan.FromHours(base.GetRandomWaitTime()), cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
            }
        }

    }
}