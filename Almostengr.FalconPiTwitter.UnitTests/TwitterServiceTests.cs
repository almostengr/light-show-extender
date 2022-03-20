using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.Clients;
using Almostengr.FalconPiTwitter.Common;
using Almostengr.FalconPiTwitter.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Tweetinvi;

namespace Almostengr.FalconPiTwitter.UnitTests
{
    public class TwitterServiceTests
    {
        private readonly ILogger<TwitterService> _tsLogger;
        private readonly AppSettings _appSettings;
        private readonly ITwitterClient _twitterClient;
        private readonly ITwitterService _twitterService;

        public TwitterServiceTests()
        {
            _tsLogger = new Logger<TwitterService>(new LoggerFactory());
            _appSettings = new AppSettings();
            _twitterService = new TwitterService(_tsLogger, _appSettings, _twitterClient);
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetRandomChristmasHashTag_Returns_StringWithChristmas()
        {
            // Setup (Arrange)

            // Attempt (Act)
            string result = _twitterService.GetRandomChristmasHashTag();

            // Verify (Assert)
            Assert.IsTrue(result.ToUpper().Contains("CHRISTMAS"));
        }

        [Test]
        public void GetRandomNewYearHashTag_Returns_StringWithNewYear()
        {
            // Setup (Arrange)

            // Attempt (Act)
            string result = _twitterService.GetRandomNewYearHashTag();

            // Verify (Assert)
            Assert.IsTrue(result.ToUpper().Contains("NEWYEAR"));
        }

        [Test]
        public void GetRandomNewYearHashTag_Returns_StringWithNewYear_WithMoq()
        {
            Mock<TwitterService> mockTwitterService = new Mock<TwitterService>();

            string result = mockTwitterService.Object.GetRandomNewYearHashTag();

            Assert.IsTrue(result.ToUpper().Contains("NEWYEAR"));
        }

        [Test]
        public async Task PostCurrentSongAsync_Returns_TweetText()
        {
            // Setup (Arrange)
            string previousTitle = "Pushing P";
            string currentTitle = "WAP (ft. Cardi B)";
            string artist = "Megan Thee Stallion";
            string playlist = "Christmas";

            // Attempt (Act)
            var result = await _twitterService.PostCurrentSongAsync(previousTitle, currentTitle, artist, playlist);

            // Verify (Assert)
            Assert.AreEqual(currentTitle, result);
        }
    }
}