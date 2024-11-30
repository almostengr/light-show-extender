using Almostengr.Common.Command;

namespace Almostengr.LightShowExtender.Worker.DomainService;

public sealed class ShowStartupCommandHandler : ICommandHandler<ShowStartupCommand, ShowStartupResponse>
{
    public async Task<ShowStartupResponse> ExecuteAsync(CancellationToken cancellationToken, ShowStartupCommand command)
    {
        // turn off driveway lights
        // turn on live stream
        await Task.CompletedTask;
        return new ShowStartupResponse();
    }
}