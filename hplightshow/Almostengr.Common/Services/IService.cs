using Almostengr.Common.OperationResult;
using Almostengr.Common.Resources;

namespace Almostengr.Common.Services;

public interface IService<TResource> where TResource : BaseResource
{
    public Task<Result<TResource>> ExecuteAsync(TResource request);
}