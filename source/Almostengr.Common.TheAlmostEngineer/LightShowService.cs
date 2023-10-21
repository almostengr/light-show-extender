namespace Almostengr.Common.TheAlmostEngineer;

public sealed class LightShowService : ILightShowService
{
    private readonly ILightShowHttpClient _httpClient;

    public LightShowService(ILightShowHttpClient httpClient)
    {
        _httpClient = httpClient  ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task DeleteSongsInQueueAsync(CancellationToken cancellationToken)
    {
        await _httpClient.DeleteSongsInQueueAsync(cancellationToken);
    }

    public async Task<LightShowDisplayResponse> GetNextSongInQueueAsync(CancellationToken cancellationToken)
    {
        return await _httpClient.GetNextSongInQueueAsync(cancellationToken);
    }

    public async Task<LightShowDisplayResponse> PostDisplayInfoAsync(LightShowDisplayRequest request, CancellationToken cancellationToken)
    {
        return await _httpClient.PostDisplayInfoAsync(request, cancellationToken);
    }

    public async Task DeleteQueueWhenPlaylistStartsAsync(string previousSong, string currentSong, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(previousSong) && !string.IsNullOrWhiteSpace(currentSong))
        {
            await DeleteSongsInQueueAsync(cancellationToken);
        }
    }
}

public interface ILightShowService
{

    Task<LightShowDisplayResponse> PostDisplayInfoAsync(LightShowDisplayRequest request, CancellationToken cancellationToken);
    Task<LightShowDisplayResponse> GetNextSongInQueueAsync(CancellationToken cancellationToken);
    Task DeleteSongsInQueueAsync(CancellationToken cancellationToken);
    Task DeleteQueueWhenPlaylistStartsAsync(string previousSong, string currentSong, CancellationToken cancellationToken);
}
