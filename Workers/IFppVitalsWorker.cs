using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiMonitor.Models;

namespace Almostengr.FalconPiTwitter.Workers
{
    public interface IFppVitalsWorker
    {
        Task StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
        Task TweetAlarmAsync(string alarmMessage);
        Task<int> IsCpuTemperatureHighAsync(IList<FalconFppdStatusSensor> sensors);
        int ResetAlarmCount(int previousHour);
    }
}