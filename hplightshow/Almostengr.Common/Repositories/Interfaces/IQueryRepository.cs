using System.Linq.Expressions;

namespace Almostengr.Common.Repositories.Interfaces;

public interface IQueryRepository<TEntity> where TEntity : BaseEntity
{
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity> GetByIdAsync(Guid id);
    Task<bool> ExistsByIdAsync(Guid id);
}
