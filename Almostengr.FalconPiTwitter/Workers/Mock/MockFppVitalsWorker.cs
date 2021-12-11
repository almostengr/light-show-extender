using System.Collections.Generic;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.DataTransferObjects;
using Microsoft.Extensions.Logging;

namespace Almostengr.FalconPiTwitter.Workers
{
    public class MockFppVitalsWorker : MockBaseWorker, IFppVitalsWorker
    {
        public MockFppVitalsWorker(ILogger<MockBaseWorker> logger) : base(logger)
        {
        }

        public Task<int> IsCpuTemperatureHighAsync(IList<FalconFppdStatusSensor> sensors)
        {
            throw new System.NotImplementedException();
        }

        public int ResetAlarmCount(int previousHour)
        {
            throw new System.NotImplementedException();
        }

        public Task TweetAlarmAsync(string alarmMessage)
        {
            throw new System.NotImplementedException();
        }
    }
}