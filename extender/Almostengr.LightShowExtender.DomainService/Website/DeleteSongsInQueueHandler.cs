using Almostengr.LightShowExtender.DomainService.Website.Common;

namespace Almostengr.LightShowExtender.DomainService.Website;

public static class DeleteSongsInQueueHandler
{
    public static async Task Handle(IWebsiteHttpClient websiteHttpClient, CancellationToken cancellationToken)
    {
        await websiteHttpClient.DeleteSongsInQueueAsync(cancellationToken);
    }
}
