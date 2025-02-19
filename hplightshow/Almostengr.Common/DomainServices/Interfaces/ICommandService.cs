using Almostengr.Common.DomainServices.Results;

namespace Almostengr.Common.DomainServices.Interfaces;

public interface ICommandService<TResource> where TResource : BaseResource
{
    public Task<Result<TResource>> ExecuteAsync(TResource resource, bool commitTransaction = true);
}
