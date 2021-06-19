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
        private ILogger<CountdownWorker> _logger;
        private ITwitterClient _twitterClient;
        private AppSettings _appSettings;
        private HttpClient _httpClient;
        private DateTime currentDate = DateTime.Now;
        private const string hashTags = "#Christmas #ChristmasCountdown #ChristmasIsComing";

        private readonly DateTime newYearDate; 
        private readonly DateTime christmasDate;

        public CountdownWorker(ILogger<CountdownWorker> logger, AppSettings appSettings, ITwitterClient twitterClient) :
            base(logger, appSettings, twitterClient)
        {
            _twitterClient = twitterClient;
            _logger = logger;
            _appSettings = appSettings;

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = HostUri;

            newYearDate = new DateTime(currentDate.Year+1, 01, 01, 00, 00, 00);
            christmasDate = new DateTime(currentDate.Year, 12, 25, 00, 00, 00);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting countdown worker");
            Random random = new Random();

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    int waitHours = random.Next(1, 12);
                    _logger.LogInformation("Waiting " + waitHours + " hours");
                    await Task.Delay(TimeSpan.FromHours(waitHours));
                    
                    TimeSpan currentTime = DateTime.Now.TimeOfDay;

                    FalconFppdStatus status = await GetCurrentStatusAsync(_httpClient);

                    bool isOfflinePlaylist = IsNextPlaylistOffline(status.Next_Playlist.Playlist);

                    // if during non-show and daytime hours then tweet
                    if (currentTime.Hours >= 7 && currentTime.Hours <= 16 && isOfflinePlaylist == false)
                    {
                        string tweetString = string.Empty;

                        tweetString += DaysUntilLightShow(status.Next_Playlist.Start_Time);
                        tweetString += DaysUntilChristmas();
                        tweetString += DaysUntilNewYear();

                        if (tweetString.Length > 0)
                        {
                            tweetString += hashTags;
                            tweetString = tweetString.Trim();
                            
                            await PostTweetAsync(tweetString);
                        }
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

        private string DaysUntilChristmas()
        {
            if (currentDate <= christmasDate)
            {
                string dayDiff = CalculateTimeBetween(currentDate, christmasDate);
                return $"There are {dayDiff} until Christmas. ";
            }

            return string.Empty;
        }

        private string DaysUntilNewYear()
        {
            if (currentDate <= newYearDate && currentDate >= christmasDate)
            {
                string dayDiff = CalculateTimeBetween(currentDate, newYearDate);
                return $"There are {dayDiff} until New Years. ";
            }

            return string.Empty;
        }

        private string DaysUntilLightShow(string start_Time)
        {
            string nextPlaylistDateTime = start_Time.Substring(0, start_Time.IndexOf(" - "));

            nextPlaylistDateTime = nextPlaylistDateTime.Replace(" @ ", "T");

            DateTime showStartDate = DateTime.Parse(nextPlaylistDateTime);

            if (currentDate <= showStartDate.AddHours(-36))
            {
                string dayDiff = CalculateTimeBetween(currentDate, showStartDate);
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