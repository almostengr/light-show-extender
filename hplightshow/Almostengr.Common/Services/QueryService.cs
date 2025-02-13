using Almostengr.Common.Repositories;
using Almostengr.Common.Repositories.Interfaces;
using Almostengr.Common.Services.Interfaces;

namespace Almostengr.Common.Services;

public class QueryService<TEntity, TResource> : IQueryService<TEntity, TResource> where TEntity : BaseEntity where TResource : BaseResource
{
    private readonly IQueryRepository<TEntity> _repository;

    public QueryService(IQueryRepository<TEntity> repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExistsByIdAsync(Guid id)
    {
        return await _repository.ExistsByIdAsync(id);
    }
}
