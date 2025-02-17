using Almostengr.Common.Domain;
using Almostengr.Common.DomainServices.Interfaces;

namespace Almostengr.Common.Infrastructure;

public class AddRepository<TEntity> : QueryRepository<TEntity>, IAddRepository<TEntity> where TEntity : BaseEntity
{
    protected AddRepository(IDbContext context) : base(context) { }

    public virtual async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task CommitAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
