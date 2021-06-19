using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Almostengr.FalconPiTwitter.Workers
{
    public abstract class MockBaseWorker : BackgroundService, IBaseWorker
    {
        private readonly ILogger<MockBaseWorker> _logger;

        public MockBaseWorker(ILogger<MockBaseWorker> logger)
        {
            _logger = logger;
        }

        public virtual Task<FalconFppdStatus> GetCurrentStatusAsync(HttpClient httpClient)
        {
            throw new System.NotImplementedException();
        }

        public Task<T> HttpGetAsync<T>(HttpClient httpClient, string route) where T : ModelBase
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> PostTweetAsync(string tweet)
        {
            throw new System.NotImplementedException();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new System.NotImplementedException();
        }
    }
}