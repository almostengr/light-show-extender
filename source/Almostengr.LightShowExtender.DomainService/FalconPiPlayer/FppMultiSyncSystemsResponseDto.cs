using Almostengr.LightShowExtender.DomainService.Common;

namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public sealed class FppMultiSyncSystemsResponseDto : BaseResponseDto
{
    public List<FppSystems> Systems { get; init; } = new();

    public sealed class FppSystems
    {
        public string Address { get; init; } = string.Empty;
        public string Hostname { get; init; } = string.Empty;
        public int LastSeen { get; init; }
    }
}