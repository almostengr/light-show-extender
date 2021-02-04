using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiMonitor.Models;
using Almostengr.FalconPiMonitor.ServicesBase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Almostengr.FalconPiMonitor.Services
{
    public class WeatherAlertService : WeatherBaseService
    {
        private List<string> _eventTypes;

        public WeatherAlertService(ILogger<WeatherAlertService> logger, IConfiguration configuration) : base(logger, configuration)
        {
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // TODO don't run if values are not set 

            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("Getting latest weather alerts");
                WeatherAlerts weatherAlerts = await GetCurrentWeatherAlertsAsync(WeatherZone);

                // TODO CHECK THE ALERTS AGAINST THE SETTINGS

                // TODO IF ALERT MATCHES SETTING, THEN SHUT DOWN SHOW AND SEND TWEET

                await Task.Delay(TimeSpan.FromMinutes(5));
            }
        }

        private async Task<WeatherAlerts> GetCurrentWeatherAlertsAsync(WeatherZone weatherZone)
        {
            string url = string.Concat(WeatherApiUrl, $"/alerts/active/zone/${weatherZone.Properties.Id}");
            string responseString = await GetRequestAsync(url);
            return JsonConvert.DeserializeObject<WeatherAlerts>(responseString);
        }

    }
}