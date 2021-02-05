using System;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiMonitor.Models;
using Almostengr.FalconPiMonitor.ServicesBase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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

                // TODO CHECK THE OBSERVATION DATA AGAINST THE APP SETTINGS

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