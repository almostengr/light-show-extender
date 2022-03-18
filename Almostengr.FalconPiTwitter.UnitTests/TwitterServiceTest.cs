using Almostengr.FalconPiTwitter.Clients;
using Almostengr.FalconPiTwitter.Common;
using Almostengr.FalconPiTwitter.Services;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Tweetinvi;

namespace Almostengr.FalconPiTwitter.UnitTests
{
    public class TwitterServiceTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetRandomChristmasHashTag_ReturnsString()
        {
            // Arrange
            ILogger<TwitterService> logger = new Logger<TwitterService>(new LoggerFactory());
            ILogger<MockTwitterClient> mockLogger = new Logger<MockTwitterClient>(new LoggerFactory());
            AppSettings appSettings = new AppSettings();
            ITwitterClient twitterClient = new MockTwitterClient(mockLogger);
            ITwitterService twitterService = new TwitterService(logger, appSettings, twitterClient);

            // Act
            string result = twitterService.GetRandomChristmasHashTag();

            // Assert
            Assert.IsInstanceOf<string>(result);
        }
    }
}