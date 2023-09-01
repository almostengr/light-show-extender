using Almostengr.LightShowExtender.DomainService.Common;

namespace Almostengr.LightShowExtender.DomainService.TheAlmostEngineer;

public sealed class TaeResponseDto : BaseDto
{
    public string message { get; init; }
    public int ResponseCode { get; init; }
    public Array Data { get; init; }
}