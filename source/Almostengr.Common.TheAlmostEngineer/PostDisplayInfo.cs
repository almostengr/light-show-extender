using Almostengr.Common.Utilities;

namespace Almostengr.Common.TheAlmostEngineer;

public sealed class EngineerDisplayRequestDto : BaseRequestDto
{
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

public sealed class PostDisplayInfoHandler : BaseHandler
{
    public PostDisplayInfoHandler(string apiUrl, string apiKey) : base(apiUrl, apiKey)
    {
    }

    public async Task<EngineerResponseDto> HandleAsync(EngineerDisplayRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _httpClient.HttpPostAsync<EngineerDisplayRequestDto, EngineerResponseDto>("fpp.php", request, cancellationToken);
        return result;
    }
}