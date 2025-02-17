namespace Almostengr.Common.DomainServices.Interfaces;

public interface IUpdateService<TResource> : ICommandService<TResource> where TResource : BaseResource;