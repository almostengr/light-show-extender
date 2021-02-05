using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiMonitor.Models;
using Almostengr.FalconPiMonitor.ServicesBase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Almostengr.FalconPiMonitor.Services
{
    public class FppVitalsService : FppBaseService
    {
        public FppVitalsService(ILogger<FppVitalsService> logger, IConfiguration configuration) : base(logger, configuration)
        {
        }

        private bool _temperatureAlarm { get; set; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (var fppInstance in AppSettings.FalconPiPlayers)
                {
                    try
                    {
                        FalconFppdStatus falconStatus = await GetCurrentStatusAsync(fppInstance.Hostname);
                        // await CheckSensorsAsync(falconStatus.Sensors);
                        await CheckCpuTemperature(falconStatus.Sensors, fppInstance.MaxCpuTemperature);
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
                } // end foreach

                // await Task.Delay(TimeSpan.FromMinutes(5));
                await Task.Delay(TimeSpan.FromSeconds(ExecuteDelaySeconds));
            }
        }

        private async Task CheckCpuTemperature(List<FalconFppdStatusSensor> sensors, double maxTemperature)
        {
            var sensor = sensors.Find(s => s.ValueType.ToLower() == "temperature");
            string alarmMessage = "";
            string tempAlert = string.Concat(sensor.Value.ToString(), "C, ", sensor.DegreesCToF(), "F");

            if (sensor.Value >= maxTemperature)
            {
                alarmMessage = string.Concat("High temperature alert ", tempAlert);
                logger.LogCritical(tempAlert);
            }

            if (alarmMessage.Length > 0 && _temperatureAlarm == false)
            {
                await PostTweetAsync(alarmMessage, false, false);
                _temperatureAlarm = true;
            }
            else
            {
                _temperatureAlarm = false;
            }
        }

        // private async Task CheckSensorsAsync(List<FalconFppdStatusSensor> sensors)
        // {
        //     if (Double.IsNegative(AppSettings.FalconPiPlayers.) || Double.IsNaN(AppSettings.Alarm.TempThreshold))
        //     {
        //         return;
        //     }

        //     foreach (var sensor in sensors)
        //     {
        //         string preText = null;

        //         if (sensor.ValueType.ToLower() == "temperature")
        //         {
        //             string tempAlert = string.Concat(sensor.Value.ToString(), "C, ", sensor.DegreesCToF(), "F");

        //             if (sensor.Value >= AppSettings.Alarm.TempThreshold && _temperatureAlarm == false)
        //             {
        //                 _temperatureAlarm = true;
        //                 preText = string.Concat("High temperature alert ", tempAlert);
        //                 logger.LogCritical(tempAlert);
        //             }
        //             else if (sensor.Value < AppSettings.Alarm.TempThreshold && _temperatureAlarm == true)
        //             {
        //                 _temperatureAlarm = false;
        //                 preText = string.Concat("Temperature below threshold ", tempAlert);
        //                 logger.LogWarning(tempAlert);
        //             }
        //         } // end for

        //         // print message if there is an alert to report
        //         if (string.IsNullOrEmpty(preText) == false)
        //         {
        //             await PostTweetAsync(string.Concat(AppSettings.Alarm.TwitterUser, " ", preText));
        //         }
        //     }
        // }

    } // end class
}