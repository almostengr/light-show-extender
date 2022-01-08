using Microsoft.Extensions.Logging;
using Tweetinvi;
using Tweetinvi.Client;
using Tweetinvi.Client.Tools;
using Tweetinvi.Client.V2;
using Tweetinvi.Core.Client;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Events;
using Tweetinvi.Models;

namespace Almostengr.FalconPiTwitter.Clients
{
    public class MockTwitterClient : ITwitterClient
    {
        private readonly ILogger<MockTwitterClient> _logger;

        public MockTwitterClient(ILogger<MockTwitterClient> logger)
        {
            _logger = logger;
        }

        public IAccountSettingsClient AccountSettings => throw new System.NotImplementedException();

        public IAuthClient Auth => throw new System.NotImplementedException();

        public IHelpClient Help => throw new System.NotImplementedException();

        public IExecuteClient Execute => throw new System.NotImplementedException();

        public IListsClient Lists => throw new System.NotImplementedException();

        public IMessagesClient Messages => throw new System.NotImplementedException();

        public IRateLimitsClient RateLimits => throw new System.NotImplementedException();

        public ISearchClient Search => throw new System.NotImplementedException();

        public IStreamsClient Streams => throw new System.NotImplementedException();

        public ITimelinesClient Timelines => throw new System.NotImplementedException();

        public ITrendsClient Trends => throw new System.NotImplementedException();

        public ITweetsClient Tweets => throw new System.NotImplementedException();
        // public ITweetsClient Tweets => new MockTweetsClient(_logger);

        public IUsersClient Users => throw new System.NotImplementedException();

        public IUploadClient Upload => throw new System.NotImplementedException();

        public IAccountActivityClient AccountActivity => throw new System.NotImplementedException();

        public ISearchV2Client SearchV2 => throw new System.NotImplementedException();

        public ITweetsV2Client TweetsV2 => throw new System.NotImplementedException();

        public IUsersV2Client UsersV2 => throw new System.NotImplementedException();

        public IStreamsV2Client StreamsV2 => throw new System.NotImplementedException();

        public IRawExecutors Raw => throw new System.NotImplementedException();

        public ITweetinviSettings Config => throw new System.NotImplementedException();

        public IReadOnlyTwitterCredentials Credentials => throw new System.NotImplementedException();

        public IExternalClientEvents Events => throw new System.NotImplementedException();

        public ITwitterClientFactories Factories => throw new System.NotImplementedException();

        public IJsonClient Json => throw new System.NotImplementedException();

        public IParametersValidator ParametersValidator => throw new System.NotImplementedException();

        public ITwitterRequest CreateRequest()
        {
            throw new System.NotImplementedException();
        }

        public ITwitterExecutionContext CreateTwitterExecutionContext()
        {
            throw new System.NotImplementedException();
        }
    }
}