using Almostengr.LightShowExtender.DomainService.Website.Common;

namespace Almostengr.LightShowExtender.DomainService.Website;

public sealed class DeleteSongsInQueueHandler
{
    private readonly IWebsiteHttpClient _websiteHttpClient;
    
    public DeleteSongsInQueueHandler(IWebsiteHttpClient websiteHttpClient)
    {
        _websiteHttpClient = websiteHttpClient;
    }

    public async Task Handle(CancellationToken cancellationToken)
    {
        await _websiteHttpClient.DeleteSongsInQueueAsync(cancellationToken);
    }
}
