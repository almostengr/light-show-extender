using Almostengr.Common.DomainServices;

namespace Almostengr.HpLightShow.Core.FalconPiPlayer.DomainServices;

public sealed class MultiSyncSystemsResource  : BaseResource
{
    public List<FppSystem> Systems { get; init; } = new();

    public sealed class FppSystem
    {
        public string Address { get; init; } = string.Empty;
        public string Hostname { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
    }
}
