using Almostengr.LightShowExtender.DomainService.Common;
using Almostengr.LightShowExtender.DomainService.Common.Constants;
using Almostengr.LightShowExtender.DomainService.Common.Extensions;
using Almostengr.LightShowExtender.DomainService.FalconPiPlayer;
using Almostengr.LightShowExtender.DomainService.NwsWeather;
using Almostengr.LightShowExtender.DomainService.TheAlmostEngineer;

namespace Almostengr.LightShowExtender.DomainService.Monitoring;

public sealed class MonitoringService : IMonitoringService
{
    private readonly IEngineerHttpClient _engineerHttpClient;
    private readonly IFppHttpClient _fppHttpClient;
    private readonly ILoggingService<MonitoringService> _logging;
    private readonly INwsHttpClient _nwsHttpClient;
    private readonly AppSettings _appSettings;

    public MonitoringService(IFppHttpClient fppHttpClient,
        IEngineerHttpClient engineerHttpClient,
        INwsHttpClient nwsHttpClient,
        AppSettings appSettings,
        ILoggingService<MonitoringService> logging)
    {
        _fppHttpClient = fppHttpClient;
        _engineerHttpClient = engineerHttpClient;
        _logging = logging;
        _nwsHttpClient = nwsHttpClient;
        _appSettings = appSettings;
    }

    public async Task LatestWeatherObservationAsync()
    {
        try
        {
            var latestObservation = await GetLatestObservationAsync(_appSettings.Monitoring.NwsStationId);
            await PutLatestObservationAsync(latestObservation);
        }
        catch (Exception ex)
        {
            _logging.Error(ex, ex.Message);
        }
    }

    private async Task<NwsLatestObservationDto> GetLatestObservationAsync(string stationId)
    {
        return await _nwsHttpClient.GetLatestObservation(stationId);
    }

    private async Task PutLatestObservationAsync(NwsLatestObservationDto observationDto)
    {
        var engineerSettingDto = new EngineerSettingRequestDto(
            EngineerSettingKey.NwsTempC.Value, observationDto.Properties.Temperature.Value.ToString());
        await _engineerHttpClient.UpdateSettingAsync(engineerSettingDto);

        string windChill = observationDto.Properties.WindChill.Value.ToString() ?? string.Empty;
        engineerSettingDto = new EngineerSettingRequestDto(
            EngineerSettingKey.WindChill.Value, windChill);
        await _engineerHttpClient.UpdateSettingAsync(engineerSettingDto);

        engineerSettingDto = new EngineerSettingRequestDto(
            EngineerSettingKey.NwsDescription.Value, observationDto.Properties.TextDescription);
        await _engineerHttpClient.UpdateSettingAsync(engineerSettingDto);
    }

    public async Task<TimeSpan> UpdateCpuTemperatureAsync()
    {
        try
        {
            var status = await _fppHttpClient.GetFppdStatusAsync(AppConstants.Localhost);

            if (status.Current_Song.IsNullOrWhiteSpace())
            {
                return TimeSpan.FromMinutes(15);
            }

            var settingDto = new EngineerSettingRequestDto(
                EngineerSettingKey.CpuTempC.Value,
                status.Sensors[0].Value.ToString());

            await _engineerHttpClient.UpdateSettingAsync(settingDto);

            return TimeSpan.FromMinutes(5);
        }
        catch (Exception ex)
        {
            _logging.Error(ex, ex.Message);
            return TimeSpan.FromSeconds(AppConstants.ErrorDelaySeconds);
        }
    }
}