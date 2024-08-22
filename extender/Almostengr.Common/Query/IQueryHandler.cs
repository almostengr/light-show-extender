namespace Almostengr.Common.Query;

public interface IQueryHandler
{ }

public interface IQueryHandler<IQueryResponse> : IQueryHandler
{
    Task<IQueryResponse> ExecuteAsync(CancellationToken cancellationToken);
}

public interface IQueryHandler<IQueryRequest, IQueryResponse> : IQueryHandler
{
    Task<IQueryResponse> ExecuteAsync(CancellationToken cancellationToken, IQueryRequest request);
}
