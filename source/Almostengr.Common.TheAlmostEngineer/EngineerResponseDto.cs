using Almostengr.Common.Utilities;

namespace Almostengr.Common.TheAlmostEngineer;

public sealed class EngineerResponseDto : BaseResultDto
{
    public EngineerResponseDto(string message)
    {
        Message = message;
    }

    public string Message { get; init; } = string.Empty;
}