namespace RhtServices.Common.Utilities.DomainService;

public abstract class HandlerResult : IHandlerResult
{
    public HandlerResult(bool succeeded)
    {
        Succeeded = succeeded;
    }

    public bool Succeeded { get; init; }
}