namespace RhtServices.Common.Utilities.DomainService;

public class HandlerResult : IHandlerResult
{
    public HandlerResult(bool succeeded)
    {
        Succeeded = succeeded;
        Dto = null;
    }

    public HandlerResult(bool succeeded, IHandlerDto handlerDto)
    {
        Succeeded = succeeded;
        Dto = handlerDto;
    }

    public bool Succeeded { get; init; }
    public IHandlerDto? Dto { get; init; }
}