using Almostengr.LightShowExtender.DomainService.Common;

namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public sealed class FppMultiSyncSystemsResponseDto : BaseResponseDto
{
    public List<FppSystem> Systems { get; init; } = new();

    public sealed class FppSystem
    {
        public string Address { get; init; } = string.Empty;
        public string Hostname { get; init; } = string.Empty;
    }
}