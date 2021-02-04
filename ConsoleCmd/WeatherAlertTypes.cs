using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Almostengr.FalconPiMonitor.ConsoleCmd
{
    public class WeatherAlertTypesConsoleCmd
    {
        private HttpClient _httpClient = new HttpClient();

        public async Task RunAsync()
        {
            HttpResponseMessage httpResponseMessage = 
                await _httpClient.GetAsync("https://api.weather.gov/alerts/types");
            List<string> eventTypes = 
                JsonConvert.DeserializeObject<List<string>>(httpResponseMessage.Content.ReadAsStringAsync().Result);

            foreach (var eventType in eventTypes){
                Console.WriteLine(eventType);
            }

            _httpClient.Dispose();
        }
    }
}