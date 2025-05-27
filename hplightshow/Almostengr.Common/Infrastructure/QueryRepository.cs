using Almostengr.Common.Domain;
using Almostengr.Common.DomainServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Almostengr.Common.Infrastructure;

public class QueryRepository<TEntity> : IQueryRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly IDbContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet;

    protected QueryRepository(IDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task<TEntity> GetByGuidAsync(Guid guid)
    {
        return await _dbSet.Where(i => i.Guid == guid).SingleOrDefaultAsync();
    }

    public async Task<bool> ExistsByGuidAsync(Guid guid)
    {
        return await _dbSet.Where(i => i.Guid == guid).AnyAsync();
    }

    public async Task<TEntity> GetByIdAsync(int id)
    {
        return await _dbSet.Where(i => i.Id == id).SingleOrDefaultAsync();
    }

    public async Task<bool> ExistsByIdAsync(int id)
    {
        return await _dbSet.Where(i => i.Id == id).AnyAsync();
    }
}
