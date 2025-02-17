using Almostengr.Common.Domain;
using Almostengr.Common.DomainServices.Interfaces;

namespace Almostengr.Common.DomainServices;

public class QueryService<TEntity, TResource> : IQueryService<TEntity, TResource> where TEntity : BaseEntity, new() where TResource : BaseResource, new()
{
    protected readonly IQueryRepository<TEntity> _repository;

    public QueryService(IQueryRepository<TEntity> repository)
    {
        _repository = repository;
    }

    public virtual async Task<bool> ExistsByGuidAsync(Guid guid)
    {
        return await _repository.ExistsByGuidAsync(guid);
    }

    protected virtual TResource ToResource(TEntity entity)
    {
        if (entity == null)
        {
            return null;
        }

        return new TResource();
    }

    protected virtual TEntity ToEntity(TResource resource)
    {
        if (resource == null)
        {
            return null;
        }

        return new TEntity();
    }
}
