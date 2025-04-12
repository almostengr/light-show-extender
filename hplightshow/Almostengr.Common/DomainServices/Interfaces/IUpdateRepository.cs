using Almostengr.Common.Domain;

namespace Almostengr.Common.DomainServices.Interfaces;

public interface IUpdateRepository<TEntity> : IAddRepository<TEntity> where TEntity : BaseDomainEntity
{
    void Update(TEntity entity);
}
