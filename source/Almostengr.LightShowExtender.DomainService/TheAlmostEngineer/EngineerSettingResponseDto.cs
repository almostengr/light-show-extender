namespace Almostengr.LightShowExtender.DomainService.TheAlmostEngineer;

public sealed class EngineerSettingResponseDto : EngineerResponseDto
{
    public string Key { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
}