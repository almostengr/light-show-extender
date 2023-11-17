namespace Almostengr.Extensions;

public interface ICommandHandler<TCommand>
{
    Task ExecuteAsync(TCommand command, CancellationToken cancellationToken);
}
