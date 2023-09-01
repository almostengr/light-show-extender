using Almostengr.LightShowExtender.DomainService.Common;
using Almostengr.LightShowExtender.DomainService.Common.Constants;
using Almostengr.LightShowExtender.DomainService.Common.Extensions;
using Almostengr.LightShowExtender.DomainService.FalconPiPlayer;
using Almostengr.LightShowExtender.DomainService.TheAlmostEngineer;

namespace Almostengr.LightShowExtender.DomainService.Monitoring;

public sealed class MonitoringService : IMonitoringService
{
    private readonly ITaeHttpClient _taeHttpClient;
    private readonly IFppHttpClient _fppHttpClient;
    private readonly ILoggingService<MonitoringService> _logging;

    public MonitoringService(IFppHttpClient fppHttpClient,
        ITaeHttpClient taeHttpClient,
        ILoggingService<MonitoringService> logging)
    {
        _fppHttpClient = fppHttpClient;
        _taeHttpClient = taeHttpClient;
        _logging = logging;
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

            var settingDto = new TaeSettingDto(
                TaeSettingKey.CpuTemperature.Value, status.Sensors[0].Value.ToString());

            await _taeHttpClient.UpdateSettingAsync(settingDto);

            return TimeSpan.FromMinutes(5);
        }
        catch (Exception ex)
        {
            _logging.Error(ex, ex.Message);
            return TimeSpan.FromSeconds(AppConstants.ErrorDelaySeconds);
        }
    }
}