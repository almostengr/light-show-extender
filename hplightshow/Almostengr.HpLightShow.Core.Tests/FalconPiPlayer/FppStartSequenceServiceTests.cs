using Moq;
using Almostengr.HpLightShow.Core.FalconPiPlayer.Infrastructure;
using Almostengr.HpLightShow.Core.Common;
using Almostengr.HpLightShow.Core.FalconPiPlayer.Services;
using Almostengr.HpLightShow.Core.Resources;
using Almostengr.HpLightShow.Core.FalconPiPlayer.Enums;


namespace Almostengr.HpLightShow.Core.Tests.FalconPiPlayer;

public class FppStartSequenceServiceTests
{
    private readonly Mock<IFppClient> _mockFppClient;
    private readonly AppSettings _appSettings;
    private readonly FppStartSequenceService _service;

    public FppStartSequenceServiceTests()
    {
        _mockFppClient = new Mock<IFppClient>();

        _appSettings = new AppSettings
        {
            SequenceOverride = null,
            FatTuesdayDate = new DateOnly(2024, 2, 13),
            EasterStartDate = new DateOnly(2024, 3, 31),
            EasterEndDate = new DateOnly(2024, 4, 1),
        };

        _service = new FppStartSequenceService(_appSettings, _mockFppClient.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowException_WhenRequestIsNull()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _service.ExecuteAsync(null!));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldUseSequenceOverride_WhenSet()
    {
        // Arrange
        var appSettings = new AppSettings
        {
            SequenceOverride = "CustomSequence",
            FatTuesdayDate = new DateOnly(2024, 2, 13),
            EasterStartDate = new DateOnly(2024, 3, 31),
            EasterEndDate = new DateOnly(2024, 4, 1),
        };
        var request = new SequenceSelectorResource(new DateOnly(2024, 7, 4));

        // Act
        await _service.ExecuteAsync(request);

        // Assert
        _mockFppClient.Verify(client => client.StartPlaylistAsync("CustomSequence"), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldSelectCorrectSequence_BasedOnRules()
    {
        // Arrange
        var request = new SequenceSelectorResource(new DateOnly(2024, 12, 25));

        // Act
        await _service.ExecuteAsync(request);

        // Assert
        _mockFppClient.Verify(client => client.StartPlaylistAsync(LightingSequence.ChristmasShow.Value), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldUseDefaultSequence_WhenNoRuleMatches()
    {
        // Arrange
        var request = new SequenceSelectorResource(new DateOnly(2024, 9, 15));

        // Act
        await _service.ExecuteAsync(request);

        // Assert
        _mockFppClient.Verify(client => client.StartPlaylistAsync(LightingSequence.Blue.Value), Times.Once);
    }
}
