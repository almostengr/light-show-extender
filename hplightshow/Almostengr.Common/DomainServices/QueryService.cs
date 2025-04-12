using Almostengr.Common.Domain;
using Almostengr.Common.DomainServices.Interfaces;

namespace Almostengr.Common.DomainServices;

public class QueryService<TEntity, TResource> : IQueryService<TEntity, TResource> 
    where TEntity : BaseDomainEntity, new() 
    where TResource : BaseDomainResource, new()
{
    protected readonly IQueryRepository<TEntity> _repository;
    protected readonly IMapper<TEntity, TResource> _mapper;

    public QueryService(
        IMapper<TEntity, TResource> mapper,
        IQueryRepository<TEntity> repository
        )
    {
        _mapper = mapper;
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
        var entities = await _repository.GetAllAsync();
        return entities.Select(_mapper.ToResource).ToArray();
    }

    public virtual async Task<TResource> GetByGuidAsync(Guid guid)
    {
        var entity = await _repository.GetByGuidAsync(guid);
        return _mapper.ToResource(entity);
    }

    public async Task<TResource> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return _mapper.ToResource(entity);
    }
}
