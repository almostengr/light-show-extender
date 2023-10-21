using Almostengr.Extensions;

namespace Almostengr.Common.TheAlmostEngineer;

public sealed class LightShowDisplayRequest : BaseRequest
{
    public LightShowDisplayRequest(string title)
    {
        Title = title;
    }

    public LightShowDisplayRequest(
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
