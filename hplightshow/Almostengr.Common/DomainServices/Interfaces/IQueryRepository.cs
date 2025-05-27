using System.Linq.Expressions;
using Almostengr.Common.Domain;

namespace Almostengr.Common.DomainServices.Interfaces;

public interface IQueryRepository<TEntity> where TEntity : BaseEntity
{
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity> GetByGuidAsync(Guid guid);
    Task<bool> ExistsByGuidAsync(Guid guid);
    Task<TEntity> GetByIdAsync(int id);
    Task<bool> ExistsByIdAsync(int id);
}
