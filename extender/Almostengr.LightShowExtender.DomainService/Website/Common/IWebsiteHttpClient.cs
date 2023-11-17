namespace Almostengr.LightShowExtender.DomainService.Website.Common;

public interface IWebsiteHttpClient
{
    Task DeleteSongsInQueueAsync(CancellationToken cancellationToken);
    Task<LightShowDisplayResponse> GetNextSongInQueueAsync(CancellationToken cancellationToken);
    Task<LightShowDisplayResponse>PostDisplayInfoAsync(WebsiteDisplayInfoRequest request, CancellationToken cancellationToken);
}