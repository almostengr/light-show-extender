using Moq;
using RhtServices.FalconPiPlayer.DomainService;
using RhtServices.HpLightShow.Core.DomainHandler.SequenceSelector;

namespace RhtServices.HpLightShow.Core.Tests;

public sealed class SequenceSelectorHandlerTests
{
    [Fact]
    public async Task ExecuteAsync_StartShow()
    {
        var fppHttpClient = new Mock<IFppHttpClient>();

        var date = DateOnly.FromDateTime(DateTime.Now);
        var request = new SequenceSelectorDto(date);
        var handler = new SequenceSelectorHandler(fppHttpClient.Object);

        var result = await handler.ExecuteAsync(request);

        Assert.True(result.Succeeded);
    }
}