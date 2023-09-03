using Almostengr.LightShowExtender.DomainService.Common;

namespace Almostengr.LightShowExtender.DomainService.TheAlmostEngineer;

public sealed class EngineerSettingRequestDto : BaseDto
{
    public EngineerSettingRequestDto(string key, string value)
    {
        Key = key;
        Value = value;
    }

    public string Key { get; init; }
    public string Value { get; init; }
}