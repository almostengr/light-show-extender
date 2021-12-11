using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.DataTransferObjects;

namespace Almostengr.FalconPiTwitter.Workers
{
    public interface IBaseWorker
    {
        Task<FalconFppdStatusDto> GetCurrentStatusAsync(HttpClient httpClient);
        Task<T> HttpGetAsync<T>(HttpClient httpClient, string route) where T : BaseDto;
        Task StartAsync(CancellationToken cancellationToken);
        Task<bool> PostTweetAsync(string tweet);
    }
}