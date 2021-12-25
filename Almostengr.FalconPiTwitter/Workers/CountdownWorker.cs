using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.Constants;
using Almostengr.FalconPiTwitter.DataTransferObjects;
using Almostengr.FalconPiTwitter.Settings;
using Microsoft.Extensions.Logging;
using Tweetinvi;

namespace Almostengr.FalconPiTwitter.Workers
{
    public class CountdownWorker : BaseWorker
    {
        private readonly ILogger<CountdownWorker> _logger;
        private readonly HttpClient _httpClient;

        public CountdownWorker(ILogger<CountdownWorker> logger, AppSettings appSettings, ITwitterClient twitterClient) :
            base(logger, appSettings, twitterClient)
        {
            _logger = logger;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = AppConstants.Localhost;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting countdown worker");
            DateTime christmasDate, currentDateTime;

            while (!stoppingToken.IsCancellationRequested)
            {
                currentDateTime = DateTime.Now;
                christmasDate = new DateTime(currentDateTime.Year, 12, 25, 00, 00, 00);
                string tweetString = string.Empty;
                FalconFppdStatusDto status = null;

                try
                {
                    status = await GetFppdStatusAsync(_httpClient);
                    tweetString += DaysUntilLightShow(currentDateTime, status.Next_Playlist.Start_Time);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }

                try
                {
                    if (IsPlaylistIdleOfflineOrTesting(status))
                    {
                        tweetString += DaysUntilChristmas(currentDateTime, christmasDate);
                        tweetString += GetRandomChristmasHashTag();

                        await PostTweetAsync(tweetString);
                    }

                    double waitHours = 7 * Random.NextDouble();

                    if (waitHours < 1)
                    {
                        waitHours = 1;
                    }
                    _logger.LogInformation($"Waiting {waitHours} hours");

                    await Task.Delay(TimeSpan.FromHours(waitHours), stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    await Task.Delay(TimeSpan.FromSeconds(DelaySeconds.Short), stoppingToken);
                }
            }
        }

        private string DaysUntilChristmas(DateTime curDateTime, DateTime christmasDate)
        {
            if (curDateTime >= christmasDate)
            {
                christmasDate = new DateTime((curDateTime.Year + 1), 12, 25, 00, 00, 00);
            }

            string dayDiff = CalculateTimeBetween(curDateTime, christmasDate);
            return $"{dayDiff} until Christmas. " + GetRandomChristmasHashTag();
        }

        private string DaysUntilLightShow(DateTime currentDateTime, string startTime)
        {
            string nextPlaylistDateTime = startTime.Substring(0, startTime.IndexOf(" - "));
            nextPlaylistDateTime = nextPlaylistDateTime.Replace(" @ ", "T");

            DateTime showStartDateTime = DateTime.Parse(nextPlaylistDateTime);

            string dayDiff = CalculateTimeBetween(currentDateTime, showStartDateTime);
            return $"{dayDiff} until the next Light Show. ";
        }

        private string CalculateTimeBetween(DateTime startDate, DateTime endDate)
        {
            TimeSpan timeDiff = endDate - startDate;
            _logger.LogDebug(timeDiff.ToString());

            string output = string.Empty;
            output += (timeDiff.Days > 0 ? (timeDiff.Days + (timeDiff.Days == 1 ? " day " : " days ")) : string.Empty);
            output += (timeDiff.Hours > 0 ? (timeDiff.Hours + (timeDiff.Hours == 1 ? " hour " : " hours ")) : string.Empty);
            output += (timeDiff.Minutes > 0 ? (timeDiff.Minutes + (timeDiff.Minutes == 1 ? " minute " : " minutes ")) : string.Empty);

            return output;
        }

        public override void Dispose()
        {
            _httpClient.Dispose();
            base.Dispose();
        }

    }
}