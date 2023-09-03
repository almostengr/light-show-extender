using Almostengr.LightShowExtender.DomainService.Common;

namespace Almostengr.LightShowExtender.DomainService.TheAlmostEngineer;

public class EngineerResponseDto : BaseDto
{
    public int Code { get; init; }
    public string Message { get; init; } = string.Empty;
}