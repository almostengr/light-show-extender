namespace Almostengr.FalconPiPlayer.DomainService;

public sealed class MultiSyncSystemsType
{
    public MultiSyncSystemsType(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static readonly MultiSyncSystemsType RaspberryPi3 = new MultiSyncSystemsType("Raspberry Pi 3 B");
    public static readonly MultiSyncSystemsType Unknown = new MultiSyncSystemsType("Unknown");
    public static readonly MultiSyncSystemsType UnknownSystem = new MultiSyncSystemsType("Unknown System Type");
    public static readonly MultiSyncSystemsType WLED = new MultiSyncSystemsType("WLED");
    public static readonly MultiSyncSystemsType All = new MultiSyncSystemsType("All");
}