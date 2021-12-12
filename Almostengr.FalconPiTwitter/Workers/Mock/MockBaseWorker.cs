using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.DataTransferObjects;
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

        public virtual Task<FalconFppdStatusDto> GetFppdStatusAsync(HttpClient httpClient)
        {
            throw new System.NotImplementedException();
        }

        public Task<FalconFppdMultiSyncSystemsDto> GetMultiSyncStatusAsync(HttpClient httpClient)
        {
            throw new System.NotImplementedException();
        }

        public string GetRandomChristmasHashTag()
        {
            throw new System.NotImplementedException();
        }

        public Task<T> HttpGetAsync<T>(HttpClient httpClient, string route) where T : BaseDto
        {
            throw new System.NotImplementedException();
        }

        public bool IsPlaylistIdleOfflineOrTesting(FalconFppdStatusDto status)
        {
            throw new System.NotImplementedException();
        }

        public Task PostTweetAlarmAsync(string alarmMessage)
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