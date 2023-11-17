using Almostengr.Extensions;
using Almostengr.Extensions.Logging;
using Almostengr.LightShowExtender.DomainService.Website.Common;

namespace Almostengr.LightShowExtender.DomainService.Website;

public sealed class DeleteSongsInQueueHandler : IQueryHandler
{
    private readonly IWebsiteHttpClient _websiteHttpClient;
    private readonly ILoggingService<DeleteSongsInQueueHandler> _loggingService;

    public DeleteSongsInQueueHandler(
        IWebsiteHttpClient websiteHttpClient,
        ILoggingService<DeleteSongsInQueueHandler> loggingService
        )
    {
        _websiteHttpClient = websiteHttpClient;
        _loggingService = loggingService;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _websiteHttpClient.DeleteSongsInQueueAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _loggingService.Error(ex.Message);
        }
    }
}
