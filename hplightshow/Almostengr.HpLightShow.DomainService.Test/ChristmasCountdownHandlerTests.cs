using Almostengr.HpLightShow.DomainService.ChristmasCountdown;
using Almostengr.HpLightShow.DomainService.Common;
using Moq;
using Xunit;

namespace Almostengr.HpLightShow.DomainService.Tests;

public class ChristmasCountdownHandlerTests
{
    [Theory]
    [InlineData(new[] { 25, 12 }, new[] { 24, 12 }, "1 day ")]
    [InlineData(new[] { 25, 12 }, new[] { 23, 12 }, "2 days ")]
    [InlineData(new[] { 25, 12 }, new[] { 25, 12 }, "Today is Christmas!")]
    public async Task ExecuteAsync_ReturnsSuccessResult_WithCorrectMessage(int[] christmasDate, int[] currentDate, string expectedMessage)
    {
        // Arrange
        var mockPoster = new Mock<ISocialMediaPoster>();
        mockPoster.Setup(p => p.PostAsync(It.IsAny<string>())).Verifiable(); 

        var request = new ChristmasCountdownRequest(new DateOnly(currentDate[0], currentDate[1], 1), new DateOnly(christmasDate[0], christmasDate[1], 1));
        var handler = new ChristmasCountdownHandler(mockPoster.Object);

        // Act
        var result = await handler.ExecuteAsync(request);

        // Assert
        Assert.True(result.Succeeded);
        mockPoster.Verify(p => p.PostAsync(expectedMessage), Times.Once); 
    }

    [Fact]
    public async Task ExecuteAsync_DoesNotPostToSocialMedia_WhenDaysDifferenceIsLessThanZero()
    {
        // Arrange
        var mockPoster = new Mock<ISocialMediaPoster>();
        mockPoster.Setup(p => p.PostAsync(It.IsAny<string>())).Verifiable(); 

        var request = new ChristmasCountdownRequest(new DateOnly(26, 12, 1), new DateOnly(25, 12, 1));
        var handler = new ChristmasCountdownHandler(mockPoster.Object);

        // Act
        var result = await handler.ExecuteAsync(request);

        // Assert
        Assert.True(result.Succeeded);
        mockPoster.Verify(p => p.PostAsync(It.IsAny<string>()), Times.Never); 
    }
}