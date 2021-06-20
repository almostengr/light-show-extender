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
        private DateTime _currentDate;
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
            Random random = new Random();
            DateTime newYearDate;
            DateTime christmasDate;

            while (!stoppingToken.IsCancellationRequested)
            {
                newYearDate = new DateTime(_currentDate.Year + 1, 01, 01, 00, 00, 00);
                christmasDate = new DateTime(_currentDate.Year, 12, 25, 00, 00, 00);

                try
                {
                    int waitHours = random.Next(1, 12);
                    _logger.LogInformation("Waiting " + waitHours + " hours");
                    await Task.Delay(TimeSpan.FromHours(waitHours));

                    _currentDate = DateTime.Now;

                    TimeSpan currentTime = DateTime.Now.TimeOfDay;

                    FalconFppdStatus status = await GetCurrentStatusAsync(_httpClient);

                    bool isOfflinePlaylist = IsNextPlaylistOffline(status.Next_Playlist.Playlist);

                    // TODO only tweet if show is offline or no playlist is active

                    // if during non-show
                    if (isOfflinePlaylist == false)
                    {
                        string tweetString = string.Empty;

                        tweetString += DaysUntilLightShow(status.Next_Playlist.Start_Time);
                        tweetString += DaysUntilChristmas(christmasDate);
                        tweetString += DaysUntilNewYear(christmasDate, newYearDate);

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

        private bool IsNextPlaylistOffline(string playlistName)
        {
            playlistName = playlistName.ToLower();

            if (playlistName.Contains("offline") || playlistName.Contains("test"))
            {
                _logger.LogInformation("Next scheduled playlist is offline. Not including in countdown");
                return true;
            }

            return false;
        }

        private string DaysUntilChristmas(DateTime christmasdate)
        {
            if (_currentDate <= christmasdate)
            {
                string dayDiff = CalculateTimeBetween(_currentDate, christmasdate);
                return $"There are {dayDiff} until Christmas. " + _christmasHashTags;
            }

            return string.Empty;
        }

        private string DaysUntilNewYear(DateTime christmasDate, DateTime newYearDate)
        {
            if (_currentDate <= newYearDate && _currentDate >= christmasDate)
            {
                string dayDiff = CalculateTimeBetween(_currentDate, newYearDate);
                return $"There are {dayDiff} until New Years. " + _newYearHashTags;
            }

            return string.Empty;
        }

        private string DaysUntilLightShow(string start_Time)
        {
            string nextPlaylistDateTime = start_Time.Substring(0, start_Time.IndexOf(" - "));

            nextPlaylistDateTime = nextPlaylistDateTime.Replace(" @ ", "T");

            DateTime showStartDate = DateTime.Parse(nextPlaylistDateTime);

            if (_currentDate <= showStartDate.AddHours(-36))
            {
                string dayDiff = CalculateTimeBetween(_currentDate, showStartDate);
                return $"There are {dayDiff} until the light show. ";
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