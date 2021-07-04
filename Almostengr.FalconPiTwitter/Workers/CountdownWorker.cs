using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.Models;
using Microsoft.Extensions.Logging;
using Tweetinvi;

namespace Almostengr.FalconPiTwitter.Workers
{
    public class CountdownWorker : BaseWorker, ICountdownWorker
    {
        private readonly ILogger<CountdownWorker> _logger;
        private readonly ITwitterClient _twitterClient;
        private readonly AppSettings _appSettings;
        private readonly HttpClient _httpClient;

        public CountdownWorker(ILogger<CountdownWorker> logger, AppSettings appSettings, ITwitterClient twitterClient) :
            base(logger, appSettings, twitterClient)
        {
            _twitterClient = twitterClient;
            _logger = logger;
            _appSettings = appSettings;

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = HostUri;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting countdown worker");
            DateTime newYearDate;
            DateTime christmasDate;
            DateTime currentDateTime;

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await WaitBeforeContinueAsync();

                    currentDateTime = DateTime.Now;
                    newYearDate = new DateTime(currentDateTime.Year + 1, 01, 01, 00, 00, 00);
                    christmasDate = new DateTime(currentDateTime.Year, 12, 25, 00, 00, 00);

                    FalconFppdStatus status = await GetCurrentStatusAsync(_httpClient);

                    // if during offline playlist or no active playlist
                    if (string.IsNullOrEmpty(status.Current_PlayList.Playlist) ||
                        status.Current_PlayList.Playlist.ToLower().Contains("offline"))
                    {
                        _logger.LogInformation("Show is idle or offline");

                        string tweetString = string.Empty;

                        tweetString += DaysUntilLightShow(currentDateTime, status.Next_Playlist.Start_Time);
                        tweetString += DaysUntilChristmas(currentDateTime, christmasDate);
                        tweetString += DaysUntilNewYear(currentDateTime, christmasDate, newYearDate);
                        tweetString += GetRandomHashTag(3);

                        await PostTweetAsync(tweetString);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    await Task.Delay(TimeSpan.FromSeconds(15));
                }
            }
        }

        private async Task WaitBeforeContinueAsync()
        {
            Random random = new Random();
            double waitHours = 12 * random.NextDouble();
            _logger.LogInformation("Waiting " + waitHours + " hours");
            await Task.Delay(TimeSpan.FromHours(waitHours));
        }

        private string DaysUntilChristmas(DateTime curDateTime, DateTime christmasdate)
        {
            if (curDateTime <= christmasdate)
            {
                string dayDiff = CalculateTimeBetween(curDateTime, christmasdate);
                return $"{dayDiff} until Christmas. " + GetChristmasHashTag();
            }

            return string.Empty;
        }

        private string DaysUntilNewYear(DateTime curDateTime, DateTime christmasDate, DateTime newYearDate)
        {
            if (curDateTime <= newYearDate && curDateTime >= christmasDate)
            {
                string dayDiff = CalculateTimeBetween(curDateTime, newYearDate);
                return $"{dayDiff} until New Years. " + GetNewYearsHashTag();
            }

            return string.Empty;
        }

        private string DaysUntilLightShow(DateTime curDateTime, string start_Time)
        {
            string nextPlaylistDateTime = start_Time.Substring(0, start_Time.IndexOf(" - "));

            nextPlaylistDateTime = nextPlaylistDateTime.Replace(" @ ", "T");

            DateTime showStartDate = DateTime.Parse(nextPlaylistDateTime);

            if (curDateTime <= showStartDate.AddHours(-36))
            {
                string dayDiff = CalculateTimeBetween(curDateTime, showStartDate);
                return $"{dayDiff} until the next Light Show. ";
            }

            return string.Empty;
        }

        private string CalculateTimeBetween(DateTime startDate, DateTime endDate)
        {
            TimeSpan timeDiff = endDate - startDate;
            _logger.LogInformation(timeDiff.ToString());

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

        private string GetChristmasHashTag()
        {
            string[] hashTags = { "#ChristmasCountdown", "#ChristmasIsComing" };
            Random random = new Random();
            return hashTags[random.Next(0, hashTags.Length)];
        }

        private string GetNewYearsHashTag()
        {
            string[] hashTags = { "#HappyNewYear", "#NewYear" };
            Random random = new Random();
            return hashTags[random.Next(0, hashTags.Length)];
        }
    }
}