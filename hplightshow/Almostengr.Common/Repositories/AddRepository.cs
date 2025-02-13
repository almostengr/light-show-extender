using Almostengr.Common.Repositories.Interfaces;

namespace Almostengr.Common.Repositories;

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

// public class UserRepository : DeleteRepository<User>
// {
//     public UserRepository(IDbContext context) : base(context) { }

//     // Add any user-specific repository methods here
// }
