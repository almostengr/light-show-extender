namespace Almostengr.HpLightShow.DomainService;

public interface IHandler<TRequest, TResult>
{
    Task<TResult> ExecuteAsync(TRequest request);
}