using Almostengr.Common.Domain;

namespace Almostengr.Common.DomainServices.Interfaces;

public interface IAddRepository<TEntity> : IQueryRepository<TEntity> where TEntity : BaseEntity
{
    Task AddAsync(TEntity entity);
    Task SaveChangesAsync();
}
