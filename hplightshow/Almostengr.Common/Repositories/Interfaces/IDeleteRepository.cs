namespace Almostengr.Common.Repositories.Interfaces;

public interface IDeleteRepository<TEntity> : IUpdateRepository<TEntity> where TEntity : BaseEntity
{
    void Delete(TEntity entity);
}
