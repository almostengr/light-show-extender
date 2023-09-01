namespace Almostengr.LightShowExtender.DomainService.TheAlmostEngineer;

public sealed class TaeSettingDto
{
    public TaeSettingDto(string key, string value)
    {
        Key = key;
        Value = value;
    }

    public string Key { get; init; }
    public string Value { get; init; }
}