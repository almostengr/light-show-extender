using Almostengr.Common.Repositories;

namespace Almostengr.Common.Services.Interfaces;

public interface IQueryService<TEntity, TResource> where TEntity : BaseEntity where TResource : BaseResource
{
    Task<bool> ExistsByIdAsync(Guid id);
}