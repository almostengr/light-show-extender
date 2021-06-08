using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.Models;
using Microsoft.Extensions.Logging;

namespace Almostengr.FalconPiTwitter.Workers
{
    public class MockTwitterWorker : MockBaseWorker, ITwitterWorker
    {
        private readonly ILogger<MockTwitterWorker> _logger;

        public MockTwitterWorker(ILogger<MockTwitterWorker> logger) : base(logger)
        {
            _logger = logger;
        }

        public Task<FalconFppdStatus> GetCurrentStatusAsync(HttpClient httpClient)
        {
            throw new System.NotImplementedException();
        }

        public Task<T> HttpGetAsync<T>(HttpClient httpClient, string route) where T : ModelBase
        {
            throw new System.NotImplementedException();
        }

    }
}