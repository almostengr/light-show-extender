namespace Almostengr.NationalWeatherService;

public sealed class NwsOptions
{
    public string ApiUrl { get; init; } = string.Empty;
    public string StationId { get; init; } = string.Empty;
    public string UserAgent { get; init; } = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.61 Safari/537.36";
}