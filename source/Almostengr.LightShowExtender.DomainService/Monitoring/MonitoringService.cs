using Almostengr.LightShowExtender.DomainService.Common;
using Almostengr.LightShowExtender.DomainService.FalconPiPlayer;
using Almostengr.LightShowExtender.DomainService.NwsWeather;
using Almostengr.LightShowExtender.DomainService.TheAlmostEngineer;

namespace Almostengr.LightShowExtender.DomainService.Monitoring;

public sealed class MonitoringService : BaseService, IMonitoringService
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
            var latestObservation = await _nwsHttpClient.GetLatestObservation(_appSettings.NwsStationId);
            await PutLatestObservationAsync(latestObservation);
        }
        catch (Exception ex)
        {
            _logging.Error(ex, ex.Message);
        }
    }

    private async Task PutLatestObservationAsync(NwsLatestObservationDto observationDto)
    {
        if (observationDto == null)
        {
            throw new ArgumentNullException(nameof(observationDto));
        }

        var engineerSettingDto = new EngineerSettingRequestDto(
            EngineerSettingKey.NwsTempC.Value, observationDto.Properties.Temperature.Value.ToString());
        await _engineerHttpClient.UpdateSettingAsync(engineerSettingDto);

        string windChill = observationDto.Properties.WindChill.Value.ToString() ?? string.Empty;
        engineerSettingDto = new EngineerSettingRequestDto(
            EngineerSettingKey.WindChill.Value, windChill);
        await _engineerHttpClient.UpdateSettingAsync(engineerSettingDto);
    }

    public async Task<TimeSpan> CheckFppStatus()
    {
        try
        {
            var fppStatus = await _fppHttpClient.GetFppdStatusAsync();
            if (fppStatus.Status_Name == StatusName.Playing)
            {
                await StopCurrentPlaylistGracefullyAsync(fppStatus);
                await UpdateCpuTemperatureAsync(fppStatus);
            }

            if (fppStatus.Scheduler.CurrentPlaylist == null)
            {
                return TimeSpan.FromMinutes(15);
            }

            return TimeSpan.FromMinutes(5);
        }
        catch (Exception ex)
        {
            _logging.Error(ex, ex.Message);
            return TimeSpan.FromMinutes(5);
        }
    }

    private async Task StopCurrentPlaylistGracefullyAsync(FppStatusDto fppStatusDto)
    {
        if (fppStatusDto == null)
        {
            throw new ArgumentNullException(nameof(fppStatusDto));
        }

        TimeSpan currentTime = DateTime.Now.TimeOfDay;
        TimeSpan showEndTime = new TimeSpan(22, 00, 00);
        if (fppStatusDto.Status_Name.ToLower() == StatusName.Playing && currentTime >= showEndTime)
        {
            _logging.Warning("Stopping playlist gracefully");
            await _fppHttpClient.StopPlaylistGracefully();
        }
    }

    private async Task UpdateCpuTemperatureAsync(FppStatusDto fppStatusDto)
    {
        if (fppStatusDto == null)
        {
            throw new ArgumentNullException(nameof(fppStatusDto));
        }

        double temperature = fppStatusDto.Sensors[0].Value;
        var settingDto = new EngineerSettingRequestDto(
            EngineerSettingKey.CpuTempC.Value, temperature.ToString());

        await _engineerHttpClient.UpdateSettingAsync(settingDto);

        if (temperature >= _appSettings.FalconPlayer.MaxCpuTemperatureC)
        {
            _logging.Warning($"CPU temperature alert: {temperature.ToString()}");
        }
    }
}