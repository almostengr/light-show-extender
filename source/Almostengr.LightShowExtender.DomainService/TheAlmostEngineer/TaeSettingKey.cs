namespace Almostengr.LightShowExtender.DomainService.TheAlmostEngineer;

public sealed class TaeSettingKey
{
    public TaeSettingKey(string value)
    {
        Value = value;
    }

    public string Value { get; init; }


    public static readonly TaeSettingKey CurrentSong = new("currentsong");
    public static readonly TaeSettingKey CpuTemperature = new("cputemperature");
}