namespace Almostengr.Common.TheAlmostEngineer;

public sealed class EngineerDisplayRequestDto 
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