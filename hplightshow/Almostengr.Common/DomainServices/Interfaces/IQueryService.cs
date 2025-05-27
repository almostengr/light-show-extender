using Almostengr.Common.Domain;

namespace Almostengr.Common.DomainServices.Interfaces;

public interface IQueryService<TEntity, TResource> where TEntity : BaseEntity where TResource : BaseResource
{
    Task<IEnumerable<TResource>> GetAllAsync();
    Task<bool> ExistsByGuidAsync(Guid guid);
    Task<TResource> GetByGuidAsync(Guid guid);
    Task<bool> ExistsByIdAsync(int id);
    Task<TResource> GetByIdAsync(int id);
}
