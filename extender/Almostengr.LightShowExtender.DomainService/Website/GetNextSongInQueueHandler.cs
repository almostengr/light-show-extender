using Almostengr.LightShowExtender.DomainService.Website.Common;

namespace Almostengr.LightShowExtender.DomainService.Website;

public static class GetNextSongInQueueHandler
{
    public static async Task<LightShowDisplayResponse> Handle(IWebsiteHttpClient websiteHttpClient, CancellationToken cancellationToken)
    {
        return await websiteHttpClient.GetNextSongInQueueAsync(cancellationToken);
    }
}