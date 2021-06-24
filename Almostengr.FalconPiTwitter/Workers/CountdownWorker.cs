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
        private const string _christmasHashTags = "#Christmas #ChristmasCountdown #ChristmasIsComing";
        private const string _newYearHashTags = "#HappyNewYear #NewYear";

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
                    await WaitBeforeContinue();

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
                        tweetString += GetRandomHashTag();

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

        private async Task WaitBeforeContinue()
        {
            Random random = new Random();
            int waitHours = random.Next(1, 12);
            _logger.LogInformation("Waiting " + waitHours + " hours");
            await Task.Delay(TimeSpan.FromHours(waitHours));
        }

        private string DaysUntilChristmas(DateTime curDateTime, DateTime christmasdate)
        {
            if (curDateTime <= christmasdate)
            {
                string dayDiff = CalculateTimeBetween(curDateTime, christmasdate);
                return $"{dayDiff} until Christmas. " + _christmasHashTags;
            }

            return string.Empty;
        }

        private string DaysUntilNewYear(DateTime curDateTime, DateTime christmasDate, DateTime newYearDate)
        {
            if (curDateTime <= newYearDate && curDateTime >= christmasDate)
            {
                string dayDiff = CalculateTimeBetween(curDateTime, newYearDate);
                return $"{dayDiff} until New Years. " + _newYearHashTags;
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
                return $"{dayDiff} until the light show. ";
            }

            return string.Empty;
        }

        private string CalculateTimeBetween(DateTime startDate, DateTime endDate)
        {
            TimeSpan timeDiff = endDate - startDate;
            _logger.LogInformation(timeDiff.ToString());
            return string.Concat(timeDiff.Days, " ",
                                (timeDiff.Days == 1 ? "day" : "days"), " ",
                                timeDiff.Hours, " ",
                                (timeDiff.Hours == 1 ? "hour" : "hours"), " ",
                                timeDiff.Minutes, " ",
                                (timeDiff.Minutes == 1 ? "minute" : "minutes"));
        }

        public override void Dispose()
        {
            _httpClient.Dispose();
            base.Dispose();
        }
    }
}