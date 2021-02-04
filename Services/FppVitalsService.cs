using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiMonitor.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Almostengr.FalconPiMonitor.Services
{
    public class FppVitalsService : BaseService
    {
        public FppVitalsService(ILogger<BaseService> logger, IConfiguration configuration) : base(logger, configuration)
        {
        }

        private bool _temperatureAlarm { get; set; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    FalconFppdStatus falconStatus = await GetCurrentStatusAsync();
                    await CheckSensorsAsync(falconStatus.Sensors);

                    // await Task.Delay(TimeSpan.FromMinutes(5));
                    await Task.Delay(TimeSpan.FromSeconds(ExecuteDelaySeconds));
                }
                catch (NullReferenceException ex)
                {
                    logger.LogError(string.Concat("Null Exception. ", ex.Message));
                    logger.LogDebug(ex, ex.Message);
                }
                catch (HttpRequestException ex)
                {
                    logger.LogError(string.Concat("Http Request Exception. Are you connected to internet? ", ex.Message));
                    logger.LogDebug(ex, ex.Message);
                }
                catch (Exception ex)
                {
                    logger.LogError(string.Concat("Unspecific Exception. ", ex.Message));
                    logger.LogDebug(ex, string.Concat(ex.GetType().ToString(), " ", ex.Message));
                }
            }
        }

        private async Task CheckSensorsAsync(IList<FalconFppdStatusSensor> sensors)
        {
            if (Double.IsNegative(AppSettings.Alarm.TempThreshold) || Double.IsNaN(AppSettings.Alarm.TempThreshold))
            {
                return;
            }

            foreach (var sensor in sensors)
            {
                string preText = null;

                if (sensor.ValueType.ToLower() == "temperature")
                {
                    string tempAlert = string.Concat(sensor.Value.ToString(), "C, ", sensor.DegreesCToF(), "F");

                    if (sensor.Value >= AppSettings.Alarm.TempThreshold && _temperatureAlarm == false)
                    {
                        _temperatureAlarm = true;
                        preText = string.Concat("High temperature alert ", tempAlert);
                        logger.LogCritical(tempAlert);
                    }
                    else if (sensor.Value < AppSettings.Alarm.TempThreshold && _temperatureAlarm == true)
                    {
                        _temperatureAlarm = false;
                        preText = string.Concat("Temperature below threshold ", tempAlert);
                        logger.LogWarning(tempAlert);
                    }
                } // end for

                // print message if there is an alert to report
                if (string.IsNullOrEmpty(preText) == false)
                {
                    await PostTweetAsync(string.Concat(AppSettings.Alarm.TwitterUser, " ", preText));
                }
            }
        }

    } // end class
}