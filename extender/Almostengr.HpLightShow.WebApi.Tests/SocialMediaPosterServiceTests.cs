// using Almostengr.Common.DomainServices.Results;
// using Almostengr.HpLightShow.WebApi.Features.SocialMediaPosts.DomainServices;
// using Almostengr.HpLightShow.WebApi.Features.SocialMediaPosts.DomainServices.Resources;
// using Microsoft.Extensions.Logging;
// using Moq;
// using Tweetinvi;
// using Tweetinvi.Client;
// using Tweetinvi.Models;
// using X.Bluesky;

// namespace Almostengr.HpLightShow.WebApi.Tests.SocialMediaPosts.DomainServices;

// public class SocialMediaPosterServiceTests
// {
//     private readonly Mock<IBlueskyClient> _mockBlueskyClient;
//     private readonly Mock<ILogger<SocialMediaPosterService>> _mockLogger;
//     private readonly Mock<ITwitterClient> _mockTwitterClient;
//     private readonly Mock<ITweetsClient> _mockTweetsClient;
//     private readonly SocialMediaPosterService _sut; // System Under Test

//     public SocialMediaPosterServiceTests()
//     {
//         _mockBlueskyClient = new Mock<IBlueskyClient>();
//         _mockLogger = new Mock<ILogger<SocialMediaPosterService>>();
//         _mockTwitterClient = new Mock<ITwitterClient>();
//         _mockTweetsClient = new Mock<ITweetsClient>();
//         _mockTwitterClient.Setup(client => client.Tweets).Returns(_mockTweetsClient.Object);

//         _sut = new SocialMediaPosterService(
//             _mockBlueskyClient.Object,
//             _mockLogger.Object,
//             _mockTwitterClient.Object
//         );
//     }

//     [Fact]
//     public async Task ExecuteAsync_NullResource_ReturnsFailure()
//     {
//         // Arrange
//         SocialMediaResource resource = null;

//         // Act
//         var result = await _sut.ExecuteAsync(resource);

//         // Assert
//         Assert.False(result.Succeeded);
//         Assert.Contains("Resource is null.", result.Errors);
//         _mockBlueskyClient.VerifyNoOtherCalls();
//         _mockTwitterClient.VerifyNoOtherCalls();
//         _mockLogger.VerifyNoOtherCalls();
//     }

//     [Fact]
//     public async Task ExecuteAsync_EmptyText_ReturnsFailure()
//     {
//         // Arrange
//         var resource = new SocialMediaResource { Text = string.Empty };

//         // Act
//         var result = await _sut.ExecuteAsync(resource);

//         // Assert
//         Assert.False(result.Succeeded);
//         Assert.Contains("No message provided to post.", result.Errors);
//         _mockBlueskyClient.VerifyNoOtherCalls();
//         _mockTwitterClient.VerifyNoOtherCalls();
//         _mockLogger.VerifyNoOtherCalls();
//     }

//     [Fact]
//     public async Task ExecuteAsync_SuccessfulPosts_ReturnsSuccess()
//     {
//         // Arrange
//         var resource = new SocialMediaResource { Text = "Test post" };
//         _mockBlueskyClient.Setup(client => client.Post(It.IsAny<string>())).Returns(Task.CompletedTask);
//         _mockTweetsClient.Setup(client => client.PublishTweetAsync("Testing post message'");

//         // Act
//         var result = await _sut.ExecuteAsync(resource);

//         // Assert
//         Assert.True(result.Succeeded);
//         Assert.Empty(result.Errors);
//         _mockBlueskyClient.Verify(client => client.Post(It.Is<string>(s => s.StartsWith("Test post "))), Times.Once);
//         _mockTweetsClient.Verify(client => client.PublishTweetAsync(It.Is<string>(s => s.StartsWith("Test post ")), It.IsAny<PublishTweetOptions>()), Times.Once);
//         _mockLogger.VerifyNoOtherCalls();
//     }

//     [Fact]
//     public async Task ExecuteAsync_BlueskyPostFails_ReturnsFailureWithBlueskyError()
//     {
//         // Arrange
//         var resource = new SocialMediaResource { Text = "Test post" };
//         var blueskyException = new Exception("Bluesky post failed");
//         _mockBlueskyClient.Setup(client => client.Post(It.IsAny<string>())).ThrowsAsync(blueskyException);
//         _mockTweetsClient.Setup(client => client.PublishTweetAsync(It.IsAny<string>(), It.IsAny<PublishTweetOptions>())).Returns(Task.FromResult<ITweet>(null));

//         // Act
//         var result = await _sut.ExecuteAsync(resource);

//         // Assert
//         Assert.False(result.Succeeded);
//         Assert.Contains("Bluesky post failed", result.Errors);
//         _mockBlueskyClient.Verify(client => client.Post(It.Is<string>(s => s.StartsWith("Test post "))), Times.Once);
//         _mockTweetsClient.Verify(client => client.PublishTweetAsync(It.Is<string>(s => s.StartsWith("Test post ")), It.IsAny<PublishTweetOptions>()), Times.Once);
//         _mockLogger.Verify(
//             x => x.LogError(It.IsAny<Exception>(), It.Is<string>(m => m.Contains("Bluesky post failed"))),
//             Times.Once
//         );
//         _mockLogger.VerifyNoOtherCalls();
//     }

//     [Fact]
//     public async Task ExecuteAsync_TwitterPostFails_ReturnsFailureWithTwitterError()
//     {
//         // Arrange
//         var resource = new SocialMediaResource { Text = "Test post" };
//         var twitterException = new Exception("Twitter post failed");
//         _mockBlueskyClient.Setup(client => client.Post(It.IsAny<string>())).Returns(Task.CompletedTask);
//         _mockTweetsClient.Setup(client => client.PublishTweetAsync(It.IsAny<string>(), It.IsAny<PublishTweetOptions>())).ThrowsAsync(twitterException);

//         // Act
//         var result = await _sut.ExecuteAsync(resource);

//         // Assert
//         Assert.False(result.Succeeded);
//         Assert.Contains("Twitter post failed", result.Errors);
//         _mockBlueskyClient.Verify(client => client.Post(It.Is<string>(s => s.StartsWith("Test post "))), Times.Once);
//         _mockTweetsClient.Verify(client => client.PublishTweetAsync(It.Is<string>(s => s.StartsWith("Test post ")), It.IsAny<PublishTweetOptions>()), Times.Once);
//         _mockLogger.Verify(
//             x => x.LogError(It.IsAny<Exception>(), It.Is<string>(m => m.Contains("Twitter post failed"))),
//             Times.Once
//         );
//         _mockLogger.VerifyNoOtherCalls();
//     }

//     [Fact]
//     public async Task ExecuteAsync_BothPostsFail_ReturnsFailureWithBothErrors()
//     {
//         // Arrange
//         var resource = new SocialMediaResource { Text = "Test post" };
//         var blueskyException = new Exception("Bluesky post failed");
//         var twitterException = new Exception("Twitter post failed");
//         _mockBlueskyClient.Setup(client => client.Post(It.IsAny<string>())).ThrowsAsync(blueskyException);
//         _mockTweetsClient.Setup(client => client.PublishTweetAsync(It.IsAny<string>(), It.IsAny<PublishTweetOptions>())).ThrowsAsync(twitterException);

//         // Act
//         var result = await _sut.ExecuteAsync(resource);

//         // Assert
//         Assert.False(result.Succeeded);
//         Assert.Contains("Bluesky post failed", result.Errors);
//         Assert.Contains("Twitter post failed", result.Errors);
//         _mockBlueskyClient.Verify(client => client.Post(It.Is<string>(s => s.StartsWith("Test post "))), Times.Once);
//         _mockTweetsClient.Verify(client => client.PublishTweetAsync(It.Is<string>(s => s.StartsWith("Test post ")), It.IsAny<PublishTweetOptions>()), Times.Once);
//         _mockLogger.Verify(
//             x => x.LogError(It.IsAny<Exception>(), It.Is<string>(m => m.Contains("Bluesky post failed"))),
//             Times.Once
//         );
//         _mockLogger.Verify(
//             x => x.LogError(It.IsAny<Exception>(), It.Is<string>(m => m.Contains("Twitter post failed"))),
//             Times.Once
//         );
//         _mockLogger.VerifyNoOtherCalls();
//     }

//     [Fact]
//     public async Task PostAsync_ValidMessage_CallsExecuteAsync()
//     {
//         // Arrange
//         const string message = "Test message from PostAsync";
//         var mockResult = Result<SocialMediaResource>.Success(new SocialMediaResource { Text = message });
//         // Setup ExecuteAsync to return a specific result for verification
//         var mockExecuteAsync = new Mock<Func<SocialMediaResource, bool, Task<Result<SocialMediaResource>>>>();
//         mockExecuteAsync.Setup(f => f(It.Is<SocialMediaResource>(r => r.Text == message), true)).ReturnsAsync(mockResult);

//         // To test PostAsync in isolation, we need to re-instantiate the SUT with a way to intercept the ExecuteAsync call.
//         // One way is to use a protected virtual method in SocialMediaPosterService for ExecuteAsync and mock that.
//         // For this example, we'll verify the state passed to the real ExecuteAsync.

//         // Act
//         var result = await _sut.PostAsync(message);

//         // Assert
//         // We verify that ExecuteAsync was called with the correct resource.
//         // Due to the non-virtual nature of ExecuteAsync, direct mocking is not straightforward.
//         // We can infer the call by the side effects on the mocked dependencies.
//         _mockBlueskyClient.Verify(client => client.Post(It.Is<string>(s => s.StartsWith(message))), Times.Once);
//         _mockTweetsClient.Verify(client => client.PublishTweetAsync(It.Is<string>(s => s.StartsWith(message)), It.IsAny<PublishTweetOptions>()), Times.Once);
//         Assert.True(result.Succeeded);
//     }
// }



using Almostengr.Common.DomainServices.Results;
using Almostengr.HpLightShow.WebApi.Features.SocialMediaPosts.DomainServices;
using Almostengr.HpLightShow.WebApi.Features.SocialMediaPosts.DomainServices.Interfaces;
using Almostengr.HpLightShow.WebApi.Features.SocialMediaPosts.DomainServices.Resources;
using Microsoft.Extensions.Logging;
using Moq;
using Tweetinvi;
using Tweetinvi.Client;
using Tweetinvi.Models;
using X.Bluesky;

namespace Almostengr.HpLightShow.WebApi.Tests.Features.SocialMediaPosts.DomainServices;

public class SocialMediaPosterServiceTests
{
    private readonly Mock<IBlueskyClient> _mockBlueskyClient;
    private readonly Mock<ILogger<SocialMediaPosterService>> _mockLogger;
    private readonly Mock<ITwitterClient> _mockTwitterClient;
    private readonly Mock<ITweetsClient> _mockTweetsClient;
    private readonly SocialMediaPosterService _sut; // System Under Test

    public SocialMediaPosterServiceTests()
    {
        _mockBlueskyClient = new Mock<IBlueskyClient>();
        _mockLogger = new Mock<ILogger<SocialMediaPosterService>>();
        _mockTwitterClient = new Mock<ITwitterClient>();
        _mockTweetsClient = new Mock<ITweetsClient>();
        _mockTwitterClient.Setup(client => client.Tweets).Returns(_mockTweetsClient.Object);

        _sut = new SocialMediaPosterService(
            _mockBlueskyClient.Object,
            _mockLogger.Object,
            _mockTwitterClient.Object
        );
    }

    [Fact]
    public async Task ExecuteAsync_NullResource_ReturnsFailure()
    {
        // Arrange
        SocialMediaResource resource = null;

        // Act
        var result = await _sut.ExecuteAsync(resource);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Contains("Resource is null.", result.Errors);
        _mockBlueskyClient.VerifyNoOtherCalls();
        _mockTwitterClient.VerifyNoOtherCalls();
        _mockLogger.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ExecuteAsync_EmptyText_ReturnsFailure()
    {
        // Arrange
        var resource = new SocialMediaResource { Text = string.Empty };

        // Act
        var result = await _sut.ExecuteAsync(resource);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Contains("No message provided to post.", result.Errors);
        _mockBlueskyClient.VerifyNoOtherCalls();
        _mockTwitterClient.VerifyNoOtherCalls();
        _mockLogger.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ExecuteAsync_SuccessfulPosts_ReturnsSuccess()
    {
        // Arrange
        var resource = new SocialMediaResource { Text = "Test post" };
        _mockBlueskyClient.Setup(client => client.Post(It.IsAny<string>())).Returns(Task.CompletedTask);
        _mockTweetsClient.Setup(client => client.PublishTweetAsync(It.IsAny<string>())).Returns(Task.FromResult<ITweet>(null));

        // Act
        var result = await _sut.ExecuteAsync(resource);

        // Assert
        Assert.True(result.Succeeded);
        Assert.Empty(result.Errors);
        _mockBlueskyClient.Verify(client => client.Post(It.Is<string>(s => s.StartsWith("Test post "))), Times.Once);
        _mockTweetsClient.Verify(client => client.PublishTweetAsync(It.Is<string>(s => s.StartsWith("Test post "))), Times.Once);
        _mockLogger.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ExecuteAsync_BlueskyPostFails_ReturnsFailureWithBlueskyError()
    {
        // Arrange
        var resource = new SocialMediaResource { Text = "Test post" };
        var blueskyException = new Exception("Bluesky post failed");
        _mockBlueskyClient.Setup(client => client.Post(It.IsAny<string>())).ThrowsAsync(blueskyException);
        _mockTweetsClient.Setup(client => client.PublishTweetAsync(It.IsAny<string>())).Returns(Task.FromResult<ITweet>(null));

        // Act
        var result = await _sut.ExecuteAsync(resource);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Contains("Bluesky post failed", result.Errors);
        _mockBlueskyClient.Verify(client => client.Post(It.Is<string>(s => s.StartsWith("Test post "))), Times.Once);
        _mockTweetsClient.Verify(client => client.PublishTweetAsync(It.Is<string>(s => s.StartsWith("Test post "))), Times.Once);
        _mockLogger.Verify(
            x => x.LogError(It.IsAny<Exception>(), It.Is<string>(m => m.Contains("Bluesky post failed"))),
            Times.Once
        );
        _mockLogger.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ExecuteAsync_TwitterPostFails_ReturnsFailureWithTwitterError()
    {
        // Arrange
        var resource = new SocialMediaResource { Text = "Test post" };
        var twitterException = new Exception("Twitter post failed");
        _mockBlueskyClient.Setup(client => client.Post(It.IsAny<string>())).Returns(Task.CompletedTask);
        _mockTweetsClient.Setup(client => client.PublishTweetAsync(It.IsAny<string>())).ThrowsAsync(twitterException);

        // Act
        var result = await _sut.ExecuteAsync(resource);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Contains("Twitter post failed", result.Errors);
        _mockBlueskyClient.Verify(client => client.Post(It.Is<string>(s => s.StartsWith("Test post "))), Times.Once);
        _mockTweetsClient.Verify(client => client.PublishTweetAsync(It.Is<string>(s => s.StartsWith("Test post "))), Times.Once);
        _mockLogger.Verify(
            x => x.LogError(It.IsAny<Exception>(), It.Is<string>(m => m.Contains("Twitter post failed"))),
            Times.Once
        );
        _mockLogger.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ExecuteAsync_BothPostsFail_ReturnsFailureWithBothErrors()
    {
        // Arrange
        var resource = new SocialMediaResource { Text = "Test post" };
        var blueskyException = new Exception("Bluesky post failed");
        var twitterException = new Exception("Twitter post failed");
        _mockBlueskyClient.Setup(client => client.Post(It.IsAny<string>())).ThrowsAsync(blueskyException);
        _mockTweetsClient.Setup(client => client.PublishTweetAsync(It.IsAny<string>())).ThrowsAsync(twitterException);

        // Act
        var result = await _sut.ExecuteAsync(resource);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Contains("Bluesky post failed", result.Errors);
        Assert.Contains("Twitter post failed", result.Errors);
        _mockBlueskyClient.Verify(client => client.Post(It.Is<string>(s => s.StartsWith("Test post "))), Times.Once);
        _mockTweetsClient.Verify(client => client.PublishTweetAsync(It.Is<string>(s => s.StartsWith("Test post "))), Times.Once);
        _mockLogger.Verify(
            x => x.LogError(It.IsAny<Exception>(), It.Is<string>(m => m.Contains("Bluesky post failed"))),
            Times.Once
        );
        _mockLogger.Verify(
            x => x.LogError(It.IsAny<Exception>(), It.Is<string>(m => m.Contains("Twitter post failed"))),
            Times.Once
        );
        _mockLogger.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task PostAsync_ValidMessage_CallsExecuteAsync()
    {
        // Arrange
        const string message = "Test message from PostAsync";
        _mockBlueskyClient.Setup(client => client.Post(It.Is<string>(s => s.StartsWith(message))));
        _mockTweetsClient.Setup(client => client.PublishTweetAsync(It.Is<string>(s => s.StartsWith(message)))).ReturnsAsync(Mock.Of<ITweet>());

        // Act
        var result = await _sut.PostAsync(message);

        // Assert
        Assert.True(result.Succeeded);
        _mockBlueskyClient.Verify(client => client.Post(It.Is<string>(s => s.StartsWith(message))), Times.Once);
        _mockTweetsClient.Verify(client => client.PublishTweetAsync(It.Is<string>(s => s.StartsWith(message))), Times.Once);
    }
}