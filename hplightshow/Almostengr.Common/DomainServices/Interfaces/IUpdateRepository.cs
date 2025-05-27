using Almostengr.Common.Domain;

namespace Almostengr.Common.DomainServices.Interfaces;

public interface IUpdateRepository<TEntity> : IAddRepository<TEntity> where TEntity : BaseEntity
{
    void Update(TEntity entity);
}
