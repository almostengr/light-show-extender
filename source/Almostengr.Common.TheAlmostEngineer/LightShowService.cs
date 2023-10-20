using Almostengr.HttpClient;

namespace Almostengr.Common.TheAlmostEngineer;

public sealed class LightShowService : BaseHttpClient, ILightShowService
{
    // private readonly ILightShowHttpClient _httpClient;
    private readonly HttpClient _httpClient;
    const string FPP_PHP = "fpp.php";

    // public LightShowService(ILightShowHttpClient httpClient)
    private LightShowService(HttpClient httpClient)
    {
        // _httpClient = httpClient;
        _httpClient = httpClient  ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task DeleteSongsInQueueAsync(CancellationToken cancellationToken)
    {
        // await _httpClient.DeleteSongsInQueueAsync(cancellationToken);
        await HttpDeleteAsync<string>(FPP_PHP, cancellationToken);
    }

    public async Task<LightShowDisplayResponse> GetNextSongInQueueAsync(CancellationToken cancellationToken)
    {
        // return await _httpClient.GetNextSongInQueueAsync(cancellationToken);
        
        var result = await HttpGetAsync<LightShowDisplayResponse>(FPP_PHP, cancellationToken);
        return result;
    }

    public Task<LightShowDisplayResponse> PostDisplayInfoAsync(LightShowDisplayRequest request, CancellationToken cancellationToken)
    {
        // throw new NotImplementedException();
        var result = await HttpPostAsync<LightShowDisplayRequest, LightShowDisplayResponse>(FPP_PHP, request, cancellationToken);
        return result;
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
