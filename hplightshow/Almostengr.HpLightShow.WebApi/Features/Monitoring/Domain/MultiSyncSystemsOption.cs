namespace Almostengr.HpLightShow.WebApi.Features.Monitoring.Domain;

public sealed class MultiSyncSystemsOption(string value)
{
    public string Value { get; } = value;

    public static readonly MultiSyncSystemsOption RaspberryPi3 = new("Raspberry Pi 3 B");
    public static readonly MultiSyncSystemsOption Unknown = new("Unknown");
    public static readonly MultiSyncSystemsOption UnknownSystem = new("Unknown System Type");
    public static readonly MultiSyncSystemsOption WLED = new("WLED");
    public static readonly MultiSyncSystemsOption All = new("All");
}