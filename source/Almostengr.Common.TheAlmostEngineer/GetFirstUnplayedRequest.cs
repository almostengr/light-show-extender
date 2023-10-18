using Almostengr.Common.Utilities;

namespace Almostengr.Common.TheAlmostEngineer;

public sealed class GetFirstUnplayedRequestHandler : BaseHandler
{
    public GetFirstUnplayedRequestHandler(string apiUrl, string apiKey) : base(apiUrl, apiKey)
    {
    }

    public async Task<EngineerResponseDto> HandleAsync(CancellationToken cancellationToken)
    {
        var result = await _httpClient.HttpGetAsync<EngineerResponseDto>("fpp.php", cancellationToken);
        return result;
    }
}