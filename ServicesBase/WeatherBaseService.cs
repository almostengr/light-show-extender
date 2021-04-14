using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiMonitor.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Almostengr.FalconPiMonitor.ServicesBase
{
    public abstract class WeatherBaseService : BaseService
    {
        protected string WeatherApiUrl = "https://api.weather.gov";
        protected WeatherStation WeatherStation;
        protected WeatherZone WeatherZone;

        public WeatherBaseService(ILogger<WeatherBaseService> logger, IConfiguration configuration) : base(logger, configuration)
        {
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            WeatherStation = await GetStationByIdAsync(AppSettings.Weather.StationId);
            WeatherZone = await GetRequestAsync<WeatherZone>(WeatherStation.Properties.Forecast);
        }

        private async Task<WeatherStation> GetStationByIdAsync(string stationId)
        {
            string url = string.Concat(WeatherApiUrl, $"/stations/${stationId}");
            return await GetRequestAsync<WeatherStation>(url);
        }

        protected async Task ShutDownShowWeatherAsync()
        {
            logger.LogInformation("Weather observation alert. Stopping show gracefully.");
            string result = await GetRequestAsync<string>("/api/playlists/stopgracefully");
            await PostTweetAsync("Weather observation alert. Stopping show gracefully.", false, true);

            // /api/playlist/:PlaylistName/start	
            // await GetRequestAsync<string>(t
        }

    }
}