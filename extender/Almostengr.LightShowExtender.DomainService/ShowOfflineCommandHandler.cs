using Almostengr.Common.Command;

namespace Almostengr.LightShowExtender.Worker.DomainService;

public sealed class ShowOfflineCommandHandler : ICommandHandler<ShowOfflineCommand, ShowOfflineResponse>
{
    public async Task<ShowOfflineResponse> ExecuteAsync(CancellationToken cancellationToken, ShowOfflineCommand command)
    {
        // turn on driveway lights
        // turn off live stream
        await Task.CompletedTask;
        return new ShowOfflineResponse();
    }
}