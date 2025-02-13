using Almostengr.Common.Repositories.Interfaces;

namespace Almostengr.Common.Repositories;

public class DeleteRepository<TEntity> : UpdateRepository<TEntity>, IDeleteRepository<TEntity> where TEntity : BaseEntity
{
    protected DeleteRepository(IDbContext context) : base(context) { }

    public virtual void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }
}
