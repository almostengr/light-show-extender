using Almostengr.Common.Domain;

namespace Almostengr.Common.DomainServices.Interfaces;

public interface IMapper<TEntity, TResource> where TEntity : BaseDomainEntity where TResource : BaseResource
{
    public TResource ToResource(TEntity entity);
}
