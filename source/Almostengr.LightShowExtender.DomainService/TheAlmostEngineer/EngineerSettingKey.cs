namespace Almostengr.LightShowExtender.DomainService.TheAlmostEngineer;

public sealed class EngineerSettingKey
{
    public EngineerSettingKey(string value)
    {
        Value = value;
    }

    public string Value { get; init; }

    public static readonly EngineerSettingKey CurrentSong = new("currentsong");
}