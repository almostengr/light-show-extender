using System;
using System.Threading.Tasks;
using Almostengr.FalconPiMonitor.Models;

namespace Almostengr.FalconPiMonitor.ConsoleCmd
{
    public class WeatherAlertTypesConsoleCmd : BaseConsoleCmd
    {
        public async Task RunAsync()
        {
            // string emailAddress = System.Configuration.ConfigurationManager.AppSettings.Get("Weather:EmailAddress");

            // if (string.IsNullOrEmpty(emailAddress))
            // {
            //     Console.WriteLine("Email address needs to be provided in configuration file (appsettings.json)");
            //     return;
            // }

            string alertTypeUrl = "https://api.weather.gov/alerts/types";

            Console.WriteLine($"Getting the list of weather alert types from {alertTypeUrl}");
            Console.WriteLine();

            // HttpClient httpClient = new HttpClient();
            // httpClient.DefaultRequestHeaders.UserAgent.ParseAdd($"(myweatherapp.com, {emailAddress})");

            // HttpResponseMessage httpResponseMessage =
            //     await httpClient.GetAsync("https://api.weather.gov/alerts/types");

            // if (httpResponseMessage.IsSuccessStatusCode)
            // {
            // WeatherAlertTypes weatherAlertTypes =
            //     JsonConvert.DeserializeObject<WeatherAlertTypes>(httpResponseMessage.Content.ReadAsStringAsync().Result);

            WeatherAlertTypes weatherAlertTypes = await GetRequestAsync<WeatherAlertTypes>(alertTypeUrl);
            foreach (var alertType in weatherAlertTypes.EventTypes)
            {
                Console.WriteLine(alertType);
            }

            // }
            // else
            // {
            //     Console.WriteLine("Unable to get alert types");
            //     Console.WriteLine(httpResponseMessage.ReasonPhrase);
            // }

            // httpClient.Dispose();
        }
    }
}