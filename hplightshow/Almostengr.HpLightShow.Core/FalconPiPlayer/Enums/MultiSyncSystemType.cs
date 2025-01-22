namespace Almostengr.HpLightShow.Core.FalconPiPlayer.Enums;

public sealed class MultiSyncSystemsType(string value)
{
    public string Value { get; } = value;

    public static readonly MultiSyncSystemsType RaspberryPi3 = new("Raspberry Pi 3 B");
    public static readonly MultiSyncSystemsType Unknown = new("Unknown");
    public static readonly MultiSyncSystemsType UnknownSystem = new("Unknown System Type");
    public static readonly MultiSyncSystemsType WLED = new("WLED");
    public static readonly MultiSyncSystemsType All = new("All");
}