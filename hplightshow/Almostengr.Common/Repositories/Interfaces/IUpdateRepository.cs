namespace Almostengr.Common.Repositories.Interfaces;

public interface IUpdateRepository<TEntity> : IAddRepository<TEntity> where TEntity : BaseEntity
{
    void Update(TEntity entity);
}
