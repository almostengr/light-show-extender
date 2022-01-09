using System;
using System.Net.Http;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.DataTransferObjects;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Almostengr.FalconPiTwitter.Clients
{
    public abstract class BaseClient : IBaseClient
    {
        private readonly ILogger<BaseClient> _logger;

        public BaseClient(ILogger<BaseClient> logger)
        {
            _logger = logger;
        }

        public string AssignRoute(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                _logger.LogWarning("No address provided");
                return null;
            }

            if (address.StartsWith("http") == false)
            {
                return $"http://{address}";
            }

            return $"{address}";
        }

        public async Task<T> HttpGetAsync<T>(HttpClient httpClient, string route) where T : BaseDto
        {
            HttpResponseMessage response = await httpClient.GetAsync(route);

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                _logger.LogError(response.ReasonPhrase);
                throw new Exception(response.ReasonPhrase);
            }
        }
    }
}