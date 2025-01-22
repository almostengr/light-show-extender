namespace Almostengr.HpLightShow.Core.FalconPiPlayer.DataTransferObjects;

public sealed class MultiSyncSystemsDto
{
    public List<FppSystem> Systems { get; init; } = new();

    public sealed class FppSystem
    {
        public string Address { get; init; } = string.Empty;
        public string Hostname { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
    }
}
