using Almostengr.Common.OperationResult;

namespace Almostengr.Common.Services.Interfaces;

public interface IModifiableService<TResource> where TResource : BaseResource
{
    public Task<Result<TResource>> ExecuteAsync(TResource request, bool commitTransaction = true);
}
