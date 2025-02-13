using Almostengr.Common.OperationResult;

namespace Almostengr.Common.Services.Interfaces;

public interface IValidateService<TResource> where TResource : BaseResource
{
    Result<TResource> Execute(TResource resource);
}