using Almostengr.Common.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Almostengr.Common.Repositories;

public class QueryRepository<TEntity> : IQueryRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly IDbContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet;

    protected QueryRepository(IDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>();
    }

    public virtual async Task<TEntity> GetByIdAsync(Guid id)
    {
        return await _dbSet.Where(i => i.ReferenceId == id)
            .SingleOrDefaultAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public virtual async Task<bool> ExistsByIdAsync(Guid id)
    {
        return await _dbSet.Where(i => i.ReferenceId == id).AnyAsync();
    }
}
