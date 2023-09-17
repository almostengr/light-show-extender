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
    private NwsLatestObservationResponseDto? _observationDto;
    private DateTime? _weatherRefreshTime;

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

    public async Task<TimeSpan> UpdateSensorDataAsync()
    {
        try
        {
            FppStatusResponseDto fppStatus = await _fppHttpClient.GetFppdStatusAsync();
            if (fppStatus.Status_Name == StatusName.Playing)
            {
                await StopCurrentPlaylistGracefullyAsync(fppStatus);
            }
            else
            {
                EngineerLightShowVitalsRequestDto vitalsDto = await GetAllPlayerCpuTemperaturesAsync();
                await LatestWeatherObservationAsync();

                vitalsDto.SetNwsTempC(_observationDto!.Properties.Temperature.Value.ToDisplayTemperature());
                vitalsDto.SetWindChill(_observationDto!.Properties.WindChill.Value.ToDisplayTemperature());

                await _engineerHttpClient.PostLatestVitalsAsync(vitalsDto);
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

    private async Task LatestWeatherObservationAsync()
    {
        try
        {
            DateTime currentTime = DateTime.Now;
            if (_observationDto == null || _weatherRefreshTime < currentTime)
            {
                _observationDto = await _nwsHttpClient.GetLatestObservation(_appSettings.NwsStationId);
                _weatherRefreshTime = currentTime.AddHours(1);
            }
        }
        catch (Exception ex)
        {
            _logging.Error(ex, ex.Message);
        }
    }

    private async Task<EngineerLightShowVitalsRequestDto> GetAllPlayerCpuTemperaturesAsync()
    {
        EngineerLightShowVitalsRequestDto vitalsDto = new();

        FppMultiSyncSystemsResponseDto fppMultiSyncSystemsDto = await _fppHttpClient.GetMultiSyncSystemsAsync();
        foreach (var system in fppMultiSyncSystemsDto.Systems)
        {
            var status = await _fppHttpClient.GetFppdStatusAsync(system.Address);

            float cpuTemperature = (float)status.Sensors
                .Where(s => s.Label.ToUpper().StartsWith("CPU"))
                .Select(s => s.Value)
                .Single();

            vitalsDto.AddCpuTemperature(cpuTemperature.ToDisplayTemperature());

            if (cpuTemperature >= _appSettings.FalconPlayer.MaxCpuTemperatureC)
            {
                _logging.Warning($"CPU temperature alert: {cpuTemperature.ToString()} for host {system.Hostname}");
            }
        }

        return vitalsDto;
    }

    private async Task StopCurrentPlaylistGracefullyAsync(FppStatusResponseDto fppStatusDto)
    {
        if (fppStatusDto == null)
        {
            throw new ArgumentNullException(nameof(fppStatusDto));
        }

        TimeSpan currentTime = DateTime.Now.TimeOfDay;
        TimeSpan showEndTime = new TimeSpan(22, 15, 00);
        if (fppStatusDto.Status_Name.ToLower() == StatusName.Playing && currentTime >= showEndTime)
        {
            _logging.Warning("Stopping playlist gracefully");
            await _fppHttpClient.StopPlaylistGracefully();
        }
    }

}