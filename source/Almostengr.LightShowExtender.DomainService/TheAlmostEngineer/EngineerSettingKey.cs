namespace Almostengr.LightShowExtender.DomainService.TheAlmostEngineer;

public sealed class EngineerSettingKey
{
    public EngineerSettingKey(string value)
    {
        Value = value;
    }

    public string Value { get; init; }

    public static readonly EngineerSettingKey CpuTempC = new("cputempc");
    public static readonly EngineerSettingKey CurrentSong = new("currentsong");
    public static readonly EngineerSettingKey NwsDescription = new("nwsdescription");
    public static readonly EngineerSettingKey OutdoorTempC  = new("outdoortempc");
    public static readonly EngineerSettingKey NwsTempC = new("nwstempc");
    public static readonly EngineerSettingKey WindChill = new("windchill");
    
}