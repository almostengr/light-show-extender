using Almostengr.LightShowExtender.DomainService.Website.Common;
using Almostengr.Extensions;
using Almostengr.Extensions.Logging;

namespace Almostengr.LightShowExtender.DomainService.Website;

public sealed class PostDisplayInfoHandler : IQueryHandler<WebsiteDisplayInfoRequest, LightShowDisplayResponse>
{
    private readonly IWebsiteHttpClient _websiteHttpClient;
    private readonly ILoggingService<PostDisplayInfoHandler> _loggingService;

    public PostDisplayInfoHandler(IWebsiteHttpClient websiteHttpClient,
        ILoggingService<PostDisplayInfoHandler> loggingService)
    {
        _websiteHttpClient = websiteHttpClient;
        _loggingService = loggingService;
    }

    public async Task<LightShowDisplayResponse> ExecuteAsync(WebsiteDisplayInfoRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrWhiteSpace(request.Title))
            {
                throw new ArgumentNullException(nameof(request.Title));
            }

            _loggingService.Information($"Posting song {request.Title} - {request.Artist}");
            return await _websiteHttpClient.PostDisplayInfoAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            _loggingService.Error(ex.Message);
            return null!;
        }
    }
}

public sealed class WebsiteDisplayInfoRequest : IQueryRequest
{
    public WebsiteDisplayInfoRequest(string title, bool acceptingRequests = false)
    {
        Title = title;
        AcceptingRequests = acceptingRequests;
    }

    public WebsiteDisplayInfoRequest(
        string title, bool acceptingRequests, string weatherTemp, string cpuTemp, string artist = "", string windChill = "")
    {
        Title = title;
        NwsTemperature = weatherTemp;
        CpuTemp = cpuTemp;
        Artist = artist;
        WindChill = windChill;
        AcceptingRequests = acceptingRequests;
    }

    public string WindChill { get; init; } = string.Empty;
    public string NwsTemperature { get; init; } = string.Empty;
    public string CpuTemp { get; init; } = string.Empty;
    public string Artist { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public bool AcceptingRequests { get; init; } = false;

    public override string ToString()
    {
        string title = Title == string.Empty ? "OFFLINE" : $"Playing {Title}, {Artist}";
        return $"{title}. Outdoor Temp {NwsTemperature}. CPU Temp {CpuTemp}. Wind chill {WindChill}.";
    }
}
