using System.Threading;
using System.Threading.Tasks;

namespace Almostengr.FalconPiTwitter.Workers
{
    public interface IFppVitalsWorker
    {
        Task StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
        Task TweetAlarmAsync(string alarmMessage);
    }
}