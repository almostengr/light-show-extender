using Almostengr.Common.Domain;
using Almostengr.Common.DomainServices.Interfaces;

namespace Almostengr.Common.Infrastructure;

public class DeleteRepository<TEntity> : UpdateRepository<TEntity>, IDeleteRepository<TEntity> where TEntity : BaseEntity
{
    protected DeleteRepository(IDbContext context) : base(context) { }

    public virtual void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }
}
