using Almostengr.LightShowExtender.DomainService.Website.Common;
using Almostengr.Extensions;

namespace Almostengr.LightShowExtender.DomainService.Website;

public sealed class PostDisplayInfoHandler
{
    private readonly IWebsiteHttpClient _websiteHttpClient;

    public PostDisplayInfoHandler(IWebsiteHttpClient websiteHttpClient)
    {
        _websiteHttpClient = websiteHttpClient;    
    }

    public async Task<LightShowDisplayResponse> Handle(WebsiteDisplayInfoRequest request, CancellationToken cancellationToken)
    {
        return await _websiteHttpClient.PostDisplayInfoAsync(request, cancellationToken);
    }
}

public sealed class WebsiteDisplayInfoRequest : BaseRequest
{
    public WebsiteDisplayInfoRequest(string title)
    {
        Title = title;
    }

    public WebsiteDisplayInfoRequest(
        string title, string weatherTemp, string cpuTemp, string artist = "", string windChill = "")
    {
        Title = title;
        NwsTemperature = weatherTemp;
        CpuTemp = cpuTemp;
        Artist = artist;
        WindChill = windChill;
    }

    public string WindChill { get; init; } = string.Empty;
    public string NwsTemperature { get; init; } = string.Empty;
    public string CpuTemp { get; init; } = string.Empty;
    public string Artist { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;

    public override string ToString()
    {
        string title = Title == string.Empty ? "OFFLINE" : $"Playing {Title}, {Artist}";
        return $"{title}. Outdoor Temp {NwsTemperature}. CPU Temp {CpuTemp}. Wind chill {WindChill}.";
    }
}
