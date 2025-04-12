using Almostengr.Common.Domain;

namespace Almostengr.Common.DomainServices.Interfaces;

public interface IAddService<TEntity, TResource> : ICommandService<TResource> 
    where TResource : BaseResource, new() 
    where TEntity : BaseDomainEntity, new();
