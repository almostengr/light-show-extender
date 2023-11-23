using System.Text.Json.Serialization;
using Almostengr.Extensions;
using Almostengr.Extensions.Logging;

namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public sealed class GetStatusHandler : IQueryHandler<FppStatusRequest, FppStatusResponse>
{
    private readonly IFppHttpClient _fppHttpClient;
    private readonly ILoggingService<GetStatusHandler> _loggingService;

    public GetStatusHandler(IFppHttpClient fppHttpClient,
        ILoggingService<GetStatusHandler> loggingService)
    {
        _fppHttpClient = fppHttpClient;
        _loggingService = loggingService;
    }

    public async Task<FppStatusResponse> ExecuteAsync(FppStatusRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return await _fppHttpClient.GetFppdStatusAsync(cancellationToken, request.Hostname);
        }
        catch (Exception ex)
        {
            _loggingService.Error(ex, ex.Message);
            return null!;
        }
    }
}

public sealed class FppStatusResponse : IQueryResponse
{
    public List<Sensor> Sensors { get; init; } = new();

    [JsonPropertyName("current_song")]
    public string Current_Song { get; init; } = string.Empty;

    [JsonPropertyName("seconds_remaining")]
    public string Seconds_Remaining { get; init; } = string.Empty;

    public sealed class Sensor
    {
        public string Label { get; init; } = string.Empty;
        public double Value { get; init; }
    }
}

public sealed class FppStatusRequest : IQueryRequest
{
    public FppStatusRequest(string hostname = "")
    {
        Hostname = hostname;
    }

    public string Hostname { get; init; }
}