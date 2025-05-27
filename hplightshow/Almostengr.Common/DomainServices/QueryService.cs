using Almostengr.Common.Domain;
using Almostengr.Common.DomainServices.Interfaces;
using Almostengr.Common.Shared;

namespace Almostengr.Common.DomainServices;

public class QueryService<TEntity, TResource> : IQueryService<TEntity, TResource>
    where TEntity : BaseEntity, new()
    where TResource : BaseResource, new()
{
    protected readonly IQueryRepository<TEntity> _repository;

    public QueryService(
        IQueryRepository<TEntity> repository
        )
    {
        _repository = repository;
    }

    public virtual async Task<bool> ExistsByGuidAsync(Guid guid)
    {
        return await _repository.ExistsByGuidAsync(guid);
    }

    public async Task<bool> ExistsByIdAsync(int id)
    {
        return await _repository.ExistsByIdAsync(id);
    }

    public async Task<IEnumerable<TResource>> GetAllAsync()
    {
        IEnumerable<TEntity> entities = await _repository.GetAllAsync();
        return entities.Select(e => e.ToResource<TResource, TEntity>()).ToArray();
    }

    public virtual async Task<TResource> GetByGuidAsync(Guid guid)
    {
        var entity = await _repository.GetByGuidAsync(guid);
        return entity.ToResource<TResource, TEntity>();
    }

    public virtual async Task<TResource> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity.ToResource<TResource, TEntity>();
    }
}
