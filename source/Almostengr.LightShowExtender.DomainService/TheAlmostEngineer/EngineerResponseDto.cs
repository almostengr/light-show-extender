using Almostengr.LightShowExtender.DomainService.Common;

namespace Almostengr.LightShowExtender.DomainService.TheAlmostEngineer;

public sealed class EngineerResponseDto : BaseDto
{
    public string Message { get; init; }
    public int ResponseCode { get; init; }
    public Array Data { get; init; }
}