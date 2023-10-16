using Almostengr.LightShowExtender.DomainService.Wled;
using Almostengr.LightShowExtender.Infrastructure.Common;

namespace Almostengr.LightShowExtender.Infrastructure.Wled;

public sealed class WledHttpClient : BaseHttpClient, IWledHttpClient
{
    private readonly HttpClient _httpClient;

    public WledHttpClient()
    {
        _httpClient = new HttpClient();
    }

    public async Task<WledJsonResponseDto> GetStatusAsync(string hostname)
    {
        if (string.IsNullOrWhiteSpace(hostname))
        {
            throw new ArgumentNullException(nameof(hostname));
        }

        string route = $"{hostname}/json";
        return await HttpGetAsync<WledJsonResponseDto>(_httpClient, route);
    }

    public async Task<WledJsonResponseDto> PostStateAsync(string hostname, WledJsonStateRequestDto wledRequestDto)
    {
        if (string.IsNullOrWhiteSpace(hostname))
        {
            throw new ArgumentNullException(nameof(hostname));
        }

        string route = $"{hostname}/json/state";
        return await HttpPostAsync<WledJsonStateRequestDto, WledJsonResponseDto>(_httpClient, route, wledRequestDto);
    }
}