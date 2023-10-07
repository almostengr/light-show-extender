using Almostengr.LightShowExtender.DomainService.Common;

namespace Almostengr.LightShowExtender.DomainService.TheAlmostEngineer;

public sealed class EngineerResponseDto : BaseResponseDto
{
    public EngineerResponseDto()
    {
    }

    public EngineerResponseDto(string message)
    {
        Message = message;
    }

    public string Message { get; init; } = string.Empty;
}