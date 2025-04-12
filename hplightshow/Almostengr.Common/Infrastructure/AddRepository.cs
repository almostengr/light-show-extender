using Almostengr.Common.Domain;
using Almostengr.Common.DomainServices.Interfaces;

namespace Almostengr.Common.Infrastructure;

public class AddRepository<TEntity> : QueryRepository<TEntity>, IAddRepository<TEntity> where TEntity : BaseDomainEntity
{
    protected AddRepository(IDbContext context) : base(context) { }

    public virtual async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
