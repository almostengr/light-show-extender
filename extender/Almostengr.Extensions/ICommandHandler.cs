namespace Almostengr.Extensions;

public interface ICommandHandler
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}

public interface ICommandHandler<TCommand>
{
    Task ExecuteAsync(ICommand command, CancellationToken cancellationToken);
}

public interface ICommand
{ }
