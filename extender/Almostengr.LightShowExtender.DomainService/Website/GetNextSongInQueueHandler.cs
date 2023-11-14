using Almostengr.LightShowExtender.DomainService.Website.Common;

namespace Almostengr.LightShowExtender.DomainService.Website;

public sealed class GetNextSongInQueueHandler
{
    private readonly IWebsiteHttpClient _websiteHttpClient;

    public GetNextSongInQueueHandler(IWebsiteHttpClient websiteHttpClient)
    {
        _websiteHttpClient = websiteHttpClient;
    }

    public async Task<LightShowDisplayResponse> Handle(CancellationToken cancellationToken)
    {
        return await _websiteHttpClient.GetNextSongInQueueAsync(cancellationToken);
    }
}