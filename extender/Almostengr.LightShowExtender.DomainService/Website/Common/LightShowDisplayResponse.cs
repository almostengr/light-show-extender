using Almostengr.Extensions;

namespace Almostengr.LightShowExtender.DomainService.Website.Common;

public sealed class LightShowDisplayResponse : BaseResponse
{
    public LightShowDisplayResponse(string message)
    {
        Message = message;
    }

    public string Message { get; init; } = string.Empty;
}