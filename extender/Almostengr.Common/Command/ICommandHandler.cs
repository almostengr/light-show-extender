namespace Almostengr.Common.Command;

public interface ICommandHandler<ICommandRequest, ICommandResponse>
{
    Task<ICommandResponse> ExecuteAsync(CancellationToken cancellationToken, ICommandRequest command);
}
