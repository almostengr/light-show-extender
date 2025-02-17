using Almostengr.Common.Domain;
using Almostengr.Common.DomainServices.Interfaces;

namespace Almostengr.Common.Infrastructure;

public class UpdateRepository<TEntity> : AddRepository<TEntity>, IUpdateRepository<TEntity> where TEntity : BaseEntity
{
    protected UpdateRepository(IDbContext context) : base(context) { }

    public virtual void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }
}
