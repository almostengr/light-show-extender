using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiMonitor.Models;
using Microsoft.Extensions.Logging;
using Tweetinvi;

namespace Almostengr.FalconPiTwitter.Workers
{
    public class MockFppCurrentSongWorker : BaseWorker, IFppCurrentSongWorker
    {
        private readonly ILogger<MockFppCurrentSongWorker> _logger;

        public MockFppCurrentSongWorker(ILogger<MockFppCurrentSongWorker> logger, AppSettings appSettings,
            HttpClient httpClient, ITwitterClient twitterClient) :
            base(logger, appSettings, httpClient, twitterClient)
        {
            _logger = logger;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _httpClient.BaseAddress = new Uri("");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<FalconMediaMeta> GetCurrentSongMetaData(string currentSong)
        {
            _logger.LogInformation("");
            throw new System.NotImplementedException();
        }

        public Task<FalconFppdStatus> GetCurrentStatus()
        {
            throw new System.NotImplementedException();
        }

        public string GetSongTitle(string notFileTitle, string tagTitle)
        {
            throw new System.NotImplementedException();
        }

        public Task GetTwitterUsername()
        {
            throw new System.NotImplementedException();
        }

        public Task<string> PostCurrentSong(string previousTitle, string currentTitle, string artist, string album, string playlist)
        {
            throw new System.NotImplementedException();
        }
    }
}