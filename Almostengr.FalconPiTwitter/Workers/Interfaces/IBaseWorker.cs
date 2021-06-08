using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.Models;

namespace Almostengr.FalconPiTwitter.Workers
{
    public interface IBaseWorker
    {
        Task<FalconFppdStatus> GetCurrentStatusAsync(HttpClient httpClient);
        Task<T> HttpGetAsync<T>(HttpClient httpClient, string route) where T : ModelBase;
        Task StartAsync(CancellationToken cancellationToken);
    }
}