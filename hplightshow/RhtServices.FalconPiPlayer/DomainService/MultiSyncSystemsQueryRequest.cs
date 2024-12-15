using RhtServices.Common.Query;

namespace RhtServices.FalconPiPlayer.DomainService;

public sealed class MultiSyncSystemsQueryResponse : IQueryResponse
{
    public List<FppSystem> Systems { get; init; } = new();

    public sealed class FppSystem
    {
        public string Address { get; init; } = string.Empty;
        public string Hostname { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
    }
}
