using System.Collections.Generic;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.DataTransferObjects;

namespace Almostengr.FalconPiTwitter.Workers
{
    public interface IFppVitalsWorker : IBaseWorker
    {
        Task CheckAllSensors(IList<FalconFppdStatusSensor> sensors);
        Task CheckInstanceVitals(RemoteSystems fppInstance);
    }
}