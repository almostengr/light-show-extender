using Almostengr.Extensions.Logging;
using Almostengr.LightShowExtender.DomainService.Website.Common;

namespace Almostengr.LightShowExtender.DomainService.Website;

public sealed class GetNextSongInQueueHandler
{
    private readonly IWebsiteHttpClient _websiteHttpClient;
    private readonly ILoggingService<GetNextSongInQueueHandler> _loggingService;

    public GetNextSongInQueueHandler(
        IWebsiteHttpClient websiteHttpClient,
        ILoggingService<GetNextSongInQueueHandler> loggingService)
    {
        _websiteHttpClient = websiteHttpClient;
        _loggingService = loggingService;
    }

    public async Task<LightShowDisplayResponse> Handle(CancellationToken cancellationToken)
    {
        try
        {
            return await _websiteHttpClient.GetNextSongInQueueAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _loggingService.Error(ex.Message);
            return null;
        }
    }
}