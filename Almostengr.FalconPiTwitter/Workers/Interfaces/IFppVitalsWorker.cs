using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.Models;

namespace Almostengr.FalconPiTwitter.Workers
{
    public interface IFppVitalsWorker : IBaseWorker
    {
        Task TweetAlarmAsync(string alarmMessage);
        Task<int> IsCpuTemperatureHighAsync(IList<FalconFppdStatusSensor> sensors);
        int ResetAlarmCount(int previousHour);
    }
}