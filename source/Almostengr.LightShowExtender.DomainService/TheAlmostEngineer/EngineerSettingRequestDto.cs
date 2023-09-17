using System.Text.Json.Serialization;
using Almostengr.LightShowExtender.DomainService.Common;

namespace Almostengr.LightShowExtender.DomainService.TheAlmostEngineer;

public sealed class EngineerSettingRequestDto : BaseRequestDto
{
    public EngineerSettingRequestDto(string key, string value)
    {
        Key = key;
        Value = value;
    }

    [JsonPropertyName("key")]
    public string Key { get; init; }

    [JsonPropertyName("value")]
    public string Value { get; init; }
}