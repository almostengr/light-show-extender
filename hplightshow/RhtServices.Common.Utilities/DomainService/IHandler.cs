namespace RhtServices.Common.Utilities.DomainService;

public interface IHandler
{ }

public interface IHandler<IHandlerResult> : IHandler
{
    Task<IHandlerResult> ExecuteAsync();
}

public interface IHandler<IHandlerResource, IHandlerResult> : IHandler
{
    Task<IHandlerResult> ExecuteAsync(IHandlerResource resource);
}