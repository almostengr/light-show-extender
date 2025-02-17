namespace Almostengr.Common.DomainServices.Results;

public sealed class NotFoundResult<TValue> : Result<TValue>
{
    public NotFoundResult() : base(default, ["Not found."])
    {
    }
}