using Almostengr.Common.Domain;

namespace Almostengr.Common.DomainServices.Interfaces;

public interface IDeleteRepository<TEntity> : IUpdateRepository<TEntity> where TEntity : BaseDomainEntity
{
    void Delete(TEntity entity);
}

