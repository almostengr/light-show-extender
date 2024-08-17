namespace Almostengr.Extensions;

public interface ICommandHandler
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}

public interface ICommandHandler<ICommand>
{
    Task ExecuteAsync(CancellationToken cancellationToken, ICommand command);
}

public interface ICommand
{ }
