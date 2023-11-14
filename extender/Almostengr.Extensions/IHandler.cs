namespace Almostengr.Extensions;

public interface IHandler
{
    Task HandleAsync(CancellationToken cancellationToken);
}

public interface IHandler<TRequest> : IHandler
{
    Task HandleAsync(TRequest request, CancellationToken cancellationToken);
}

public interface IHandler<TRequest, TResponse> : IHandler<TRequest>
{
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);
}
