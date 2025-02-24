using Almostengr.Common.Domain;

namespace Almostengr.Common.DomainServices.Interfaces;

public interface IQueryService<TEntity, TResource> where TEntity : BaseEntity where TResource : BaseEntityResource
{
    Task<bool> ExistsByGuidAsync(Guid guid);
}
