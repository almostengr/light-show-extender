using Almostengr.Common.Utilities.DomainService;

namespace Almostengr.FalconPiPlayer.DomainService;

public sealed class MultiSyncSystemsQueryResponse : IHandlerDto
{
    public List<FppSystem> Systems { get; init; } = new();

    public sealed class FppSystem
    {
        public string Address { get; init; } = string.Empty;
        public string Hostname { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
    }
}
