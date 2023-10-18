namespace Almostengr.Common.TheAlmostEngineer;

public sealed class DeleteSongsInQueueHandler : BaseHandler
{
    public DeleteSongsInQueueHandler(string apiUrl, string apiKey) : base(apiUrl, apiKey)
    {
    }

    public async Task HandleAsync(CancellationToken cancellationToken)
    {
        var result = await _httpClient.HttpDeleteAsync<string>("fpp.php", cancellationToken);
    }
}