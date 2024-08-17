namespace Almostengr.FalconPiPlayer.DomainService.FalconPiPlayer;

public sealed class FppSystemType
{
    public FppSystemType(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static readonly FppSystemType RaspberryPi3 = new FppSystemType("Raspberry Pi 3 B");
    public static readonly FppSystemType Unknown = new FppSystemType("Unknown");
    public static readonly FppSystemType UnknownSystem = new FppSystemType("Unknown System Type");
    public static readonly FppSystemType WLED = new FppSystemType("WLED");
    public static readonly FppSystemType All = new FppSystemType("All");
}