using Almostengr.Common.Utilities;

namespace Almostengr.Common.TheAlmostEngineer;

public sealed class LightShowDisplayResponse : BaseResponse
{
    public LightShowDisplayResponse(string message)
    {
        Message = message;
    }

    public string Message { get; init; } = string.Empty;
}