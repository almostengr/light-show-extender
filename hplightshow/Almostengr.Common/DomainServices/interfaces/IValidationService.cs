using Almostengr.Common.DomainServices.Results;

namespace Almostengr.Common.DomainServices.Interfaces;

public interface IValidationService<TResource> where TResource : BaseResource
{
    Result<TResource> Execute(TResource resource);
}