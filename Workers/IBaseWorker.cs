using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiMonitor.Models;

namespace Almostengr.FalconPiTwitter.Workers
{
    public interface IBaseWorker
    {
        Task<FalconFppdStatus> GetCurrentStatusAsync(HttpClient httpClient);
        Task<T> HttpGetAsync<T>(HttpClient httpClient, string route) where T : class;
        Task StartAsync(CancellationToken cancellationToken);
    }
}