using Almostengr.LightShowExtender.DomainService.Common;

namespace Almostengr.LightShowExtender.DomainService.TheAlmostEngineer;

public sealed class EngineerLightShowDisplayRequestDto : BaseRequestDto
{
    public string WindChill { get; private set; } = string.Empty;
    public string NwsTemperature { get; private set; } = string.Empty;
    public string CpuTemp { get; private set; } = string.Empty;
    // private List<CpuTempSensor> _cpuTempSensors { get; set; } = new();
    // public IReadOnlyList<CpuTempSensor> CpuTempSensors { get { return _cpuTempSensors; } }
    public string Artist { get; private set; } = string.Empty;
    public string Title { get; private set; } = string.Empty;

    // public sealed class CpuTempSensor
    // {
    //     public CpuTempSensor(string displayText)
    //     {
    //         Text = displayText;
    //     }

    //     public string Text { get; private set; } = string.Empty;
    // }

    internal void AddCpuTemperature(string displayText)
    {
        if (!string.IsNullOrWhiteSpace(CpuTemp))
        {
            CpuTemp += ", ";
        }

        CpuTemp += displayText;
    }

    internal void SetWindChill(string displayText)
    {
        WindChill = displayText;
    }

    internal void SetNwsTempC(string displayText)
    {
        NwsTemperature = displayText;
    }

    internal void SetTitle(string title)
    {
        Title = title;
    }

    internal void setArtist(string artist)
    {
        Artist = artist;
    }
}