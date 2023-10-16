using Almostengr.LightShowExtender.DomainService.Common;

namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public sealed class FppMultiSyncSystemsResponseDto : BaseResponseDto
{
    public List<FppSystem> Systems { get; init; } = new();

    public sealed class FppSystem
    {
        public string Address { get; init; } = string.Empty;
        public string Hostname { get; init; } = string.Empty;
        public FppSystemType Type { get; init; } = FppSystemType.Unknown;
    }
}

public sealed class FppSystemType
{
    public FppSystemType(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static readonly FppSystemType RaspberryPi3 = new FppSystemType("Raspberry Pi 3 B");
    public static readonly FppSystemType Unknown = new FppSystemType("Unknown");
    public static readonly FppSystemType WLED = new FppSystemType("WLED");
}