using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiMonitor.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Almostengr.FalconPiMonitor.Services
{
    public class WeatherService : BaseService
    {
        private string _baseApiUrl = "https://api.weather.gov";
        private List<string> _eventTypes;

        public WeatherService(ILogger<BaseService> logger, IConfiguration configuration) : base(logger, configuration)
        {
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            WeatherStation weatherStation;
            WeatherZone weatherZone;

            weatherStation = await GetStationByIdAsync(AppSettings.Weather.StationId);
            weatherZone = await GetZoneByStationAsync(weatherStation);

            while (!stoppingToken.IsCancellationRequested)
            {
                WeatherObservationCurrent weatherObservation = await GetCurrentObservationsAsync(weatherStation);

            }
        }

        private async Task<WeatherZone> GetZoneByStationAsync(WeatherStation wStation)
        {
            string responseString = await GetRequestAsync(wStation.Properties.Forecast);
            return JsonConvert.DeserializeObject<WeatherZone>(responseString);
        }

        private async Task<WeatherObservationCurrent> GetCurrentObservationsAsync(WeatherStation wStation)
        {
            string url =
                string.Concat(_baseApiUrl, $"/stations/{wStation.Properties.StationIdentifier}/observations/latest");
            string responseString = await GetRequestAsync(url);
            return JsonConvert.DeserializeObject<WeatherObservationCurrent>(responseString);
        }

        private async Task<WeatherStation> GetStationByIdAsync(string stationId)
        {
            string url = string.Concat(_baseApiUrl, $"/stations/${stationId}");
            string responseString = await GetRequestAsync(url);
            return JsonConvert.DeserializeObject<WeatherStation>(responseString);
        }

        private async Task<WeatherAlerts> GetCurrentWeatherAlertsAsync(WeatherZone wZone)
        {
            string url = string.Concat(_baseApiUrl, $"/alerts/active/zone/${wZone.Properties.Id}");
            string responseString = await GetRequestAsync(url);
            return JsonConvert.DeserializeObject<WeatherAlerts>(responseString);
        }

    }
}