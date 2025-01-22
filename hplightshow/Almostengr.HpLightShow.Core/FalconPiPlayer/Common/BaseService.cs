using Almostengr.Common.Infrastructure;

namespace Almostengr.HpLightShow.Core.FalconPiPlayer.Common;

public abstract class BaseService
{
    protected readonly HttpClient _httpClient;

    internal BaseService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("http://127.0.0.1");
        _httpClient.Timeout = TimeSpan.FromSeconds(15);
    }

    protected async Task<string> StartPlaylistAsync(string playlist)
    {
        if (string.IsNullOrWhiteSpace(playlist))
        {
            throw new ArgumentNullException("Invalid playlist name.");
        }

        string route = $"api/command/Start Playlist/{playlist}/true/false";
        return await _httpClient.GetAsync<string>(route);
    }
}