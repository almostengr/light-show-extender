using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiMonitor.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Almostengr.FalconPiMonitor.ServicesBase
{
    public abstract class WeatherBaseService : BaseService
    {
        public string WeatherApiUrl = "https://api.weather.gov";
        public WeatherStation WeatherStation;
        public WeatherZone WeatherZone;

        public WeatherBaseService(ILogger<WeatherBaseService> logger, IConfiguration configuration) : base(logger, configuration)
        {
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            WeatherStation = await GetStationByIdAsync(AppSettings.Weather.StationId);
            WeatherZone = await GetZoneByStationAsync(WeatherStation);
        }

        private async Task<WeatherZone> GetZoneByStationAsync(WeatherStation weatherStation)
        {
            string responseString = await GetRequestAsync(weatherStation.Properties.Forecast);
            return JsonConvert.DeserializeObject<WeatherZone>(responseString);
        }

        private async Task<WeatherStation> GetStationByIdAsync(string stationId)
        {
            string url = string.Concat(WeatherApiUrl, $"/stations/${stationId}");
            string responseString = await GetRequestAsync(url);
            return JsonConvert.DeserializeObject<WeatherStation>(responseString);
        }

    }
}