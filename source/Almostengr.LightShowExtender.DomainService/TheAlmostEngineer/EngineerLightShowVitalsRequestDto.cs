using Almostengr.LightShowExtender.DomainService.Common;

namespace Almostengr.LightShowExtender.DomainService.TheAlmostEngineer;

public sealed class EngineerLightShowVitalsRequestDto : BaseRequestDto
{
    public string WindChill { get; private set; } = string.Empty;
    public string NwsTemperature { get; private set; } = string.Empty;
    private List<CpuTempSensor> _cpuTempSensors { get; set; } = new();
    public IReadOnlyList<CpuTempSensor> CpuTempSensors { get { return _cpuTempSensors; } }

    public sealed class CpuTempSensor
    {
        public CpuTempSensor(string displayText)
        {
            Text = displayText;
        }

        public string Text { get; private set; } = string.Empty;
    }

    public void AddCpuTemperature(string displayText)
    {
        CpuTempSensor sensor = new(displayText);
        _cpuTempSensors.Add(sensor);
    }

    public void SetWindChill(string displayText)
    {
        WindChill = displayText;
    }

    public void SetNwsTempC(string displayText)
    {
        NwsTemperature = displayText;
    }
}