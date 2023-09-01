namespace Almostengr.LightShowExtender.DomainService.TheAlmostEngineer;

public sealed class EngineerSettingDto
{
    public EngineerSettingDto(string key, string value)
    {
        Key = key;
        Value = value;
    }

    public string Key { get; init; }
    public string Value { get; init; }
}