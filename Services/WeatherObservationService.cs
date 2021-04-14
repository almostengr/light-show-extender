using System;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiMonitor.Models;
using Almostengr.FalconPiMonitor.ServicesBase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Almostengr.FalconPiMonitor.Services
{
    public class WeatherObservationService : WeatherBaseService
    {
        public WeatherObservationService(ILogger<WeatherObservationService> logger, IConfiguration configuration) : base(logger, configuration)
        {
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (false)
            {
                await base.ExecuteAsync(stoppingToken);
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("Getting latest weather observations");
                WeatherObservation weatherObservation = await GetCurrentObservationsAsync(WeatherStation);

                if (weatherObservation.Properties.Temperature.Value >= AppSettings.Weather.MaxTemperature ||
                    weatherObservation.Properties.WindGust.Value >= AppSettings.Weather.MaxWindSpeed ||
                    weatherObservation.Properties.WindSpeed.Value >= AppSettings.Weather.MaxWindSpeed)
                {
                    logger.LogInformation("Weather observation alert. Stopping show gracefully.");
                    string result  = await GetRequestAsync<string>("/api/playlists/stopgracefully");
                    await PostTweetAsync("Weather observation alert. Stopping show gracefully.", false, true);
                }

                await Task.Delay(TimeSpan.FromHours(1));
            }
        }
        
        private async Task<WeatherObservation> GetCurrentObservationsAsync(WeatherStation weatherStation)
        {
            string url =
                string.Concat(WeatherApiUrl, $"/stations/{weatherStation.Properties.StationIdentifier}/observations/latest");
            return await GetRequestAsync<WeatherObservation>(url);
        }

    }
}