namespace Almostengr.Common.Utilities.DomainService;

public interface IHandler
{ }

public interface IHandler<IHandlerRequest, IHandlerResult> : IHandler
{
    Task<IHandlerResult> ExecuteAsync(IHandlerRequest request);
}