using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Almostengr.FalconPiTwitter.Workers
{
    public class MockTwitterWorker : MockBaseWorker, ITwitterMentionsWorker
    {
        private readonly ILogger<MockTwitterWorker> _logger;

        public MockTwitterWorker(ILogger<MockTwitterWorker> logger) : base(logger)
        {
            _logger = logger;
        }

        public async Task LikeMentionedTweetsAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
            _logger.LogInformation("Multiple tweets liked");
        }
    }
}