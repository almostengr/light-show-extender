using Almostengr.Common.Domain;
using Almostengr.Common.DomainServices.Interfaces;

namespace Almostengr.Common.DomainServices;

public class QueryService<TEntity, TResource> : IQueryService<TEntity, TResource> where TEntity : BaseEntity, new() where TResource : BaseResource, new()
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
    
    public virtual async Task<TResource> GetByGuidAsync(Guid guid)
    {
        var entity = await _repository.GetByGuidAsync(guid);
        return _mapper.ToResource(entity);
    }
}
