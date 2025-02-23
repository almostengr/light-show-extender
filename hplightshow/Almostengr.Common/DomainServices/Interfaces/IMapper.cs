using Almostengr.Common.Domain;

namespace Almostengr.Common.DomainServices.Interfaces;

public interface IMapper<TEntity, TResource> where TEntity : BaseEntity where TResource : BaseResource
{
    public TResource ToResource(TEntity entity);
    public TEntity ToEntity(TResource resource);
}
