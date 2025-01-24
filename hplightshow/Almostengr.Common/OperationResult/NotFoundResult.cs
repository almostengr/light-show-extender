namespace Almostengr.Common.OperationResult;

public sealed class NotFoundResult<TValue> : Result<TValue>
{
    public NotFoundResult() : base(default, ["Not found."])
    {
    }
}