using System.Net.Http;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.DataTransferObjects;

namespace Almostengr.FalconPiTwitter.Clients
{
    public interface IBaseClient
    {
        string AssignRoute(string address);
        Task<T> HttpGetAsync<T>(HttpClient httpClient, string route) where T : BaseDto;
    }
}