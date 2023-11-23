namespace Almostengr.Extensions;

public interface IQueryHandler
{ }

public interface IQueryHandler<IQueryResponse> : IQueryHandler
{
    Task<IQueryResponse> ExecuteAsync(CancellationToken cancellationToken);
}

public interface IQueryHandler<IQueryRequest, IQueryResponse> : IQueryHandler
{
    Task<IQueryResponse> ExecuteAsync(IQueryRequest request, CancellationToken cancellationToken);
}

public interface IQueryRequest
{ }

public interface IQueryResponse
{ }
