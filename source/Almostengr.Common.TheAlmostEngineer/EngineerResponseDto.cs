namespace Almostengr.Common.TheAlmostEngineer;

public sealed class EngineerResponseDto
{
    public EngineerResponseDto(string message)
    {
        Message = message;
    }

    public string Message { get; init; } = string.Empty;
}