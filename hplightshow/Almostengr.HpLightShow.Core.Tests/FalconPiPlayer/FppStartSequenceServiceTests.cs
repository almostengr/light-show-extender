using Moq;
using Almostengr.HpLightShow.Core.FalconPiPlayer.DomainServices;
using Almostengr.HpLightShow.Core.FalconPiPlayer.DomainServices.Infrastructure;
using Almostengr.HpLightShow.Core.FalconPiPlayer.Domain;
using Almostengr.HpLightShow.Core.FalconPiPlayer.DomainServices.Interfaces;
using Almostengr.HpLightShow.Core.FalconPiPlayer.Shared;


namespace Almostengr.HpLightShow.Core.Tests.FalconPiPlayer;

public class FppStartSequenceServiceTests
{
    private readonly Mock<IFppClient> _mockFppClient;
    private readonly FppAppSettings _appSettings;
    private readonly FppStartSequenceService _service;
    private readonly Mock<IFppSequenceRepository> _mockRepository;

    public FppStartSequenceServiceTests()
    {
        _mockFppClient = new Mock<IFppClient>();

        _appSettings = new FppAppSettings
        {
            SequenceOverride = null,
            FatTuesdayDate = new DateOnly(2024, 2, 13),
            EasterStartDate = new DateOnly(2024, 3, 31),
            EasterEndDate = new DateOnly(2024, 4, 1),
        };

        _mockRepository = new Mock<IFppSequenceRepository>();

        _service = new FppStartSequenceService(_appSettings, _mockRepository.Object, _mockFppClient.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenRequestIsNull()
    {
        // act
        var result = await _service.ExecuteAsync(null!);

        // assert
        Assert.Null(result.Value);
        Assert.True(result.Failed);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldUseSequenceOverride_WhenSet()
    {
        // Arrange
        var appSettings = new FppAppSettings
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
