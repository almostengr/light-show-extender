using System;
using System.Collections.Generic;
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
                await TemperatureCheck(falconStatus.Sensors);

                await Task.Delay(TimeSpan.FromMinutes(5));
            }

            // return base.ExecuteAsync(stoppingToken);
        }

        private async Task TemperatureCheck(IList<FalconFppdStatusSensor> sensors)
        {
            if (Double.IsNegative(AppSettings.Alarm.TempThreshold) ||
                    Double.IsNaN(AppSettings.Alarm.TempThreshold))
            {
                return;
            }

            foreach (var sensor in sensors)
            {
                if (sensor.ValueType.ToLower() == "temperature")
                {
                    string tempAlert = string.Concat(sensor.Value.ToString(), "C, ", sensor.DegreesCToF(), "F");
                    string preText = null;

                    if (sensor.Value >= AppSettings.Alarm.TempThreshold && _temperatureAlarm == false)
                    {
                        _temperatureAlarm = true;
                        preText = "High temperature alert";
                        logger.LogCritical(tempAlert);
                    }
                    else if (sensor.Value < AppSettings.Alarm.TempThreshold && _temperatureAlarm == true)
                    {
                        _temperatureAlarm = false;
                        preText = "Temperature below threshold";
                        logger.LogWarning(tempAlert);
                    }

                    if (string.IsNullOrEmpty(preText) == false)
                    {
                        await PostTweet(string.Concat(AppSettings.Alarm.TwitterUser, " ",
                            preText, " ", tempAlert));
                    }

                    break;
                }
            }
        }
    }
}