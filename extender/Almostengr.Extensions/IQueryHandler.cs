namespace Almostengr.Extensions;

public interface IQueryHandler
{ }

public interface IQueryHandler<TResult> : IQueryHandler
{
    Task<TResult> ExecuteAsync(CancellationToken cancellationToken);
}

public interface IQueryHandler<TQuery, TResult> : IQueryHandler
{
    Task<TResult> ExecuteAsync(TQuery query, CancellationToken cancellationToken);
}
