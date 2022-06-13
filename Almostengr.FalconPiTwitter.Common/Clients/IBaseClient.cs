using Almostengr.FalconPiTwitter.DataTransferObjects;

namespace Almostengr.FalconPiTwitter.Clients
{
    public interface IBaseClient
    {
        Task<T> HttpGetAsync<T>(HttpClient httpClient, string route) where T : BaseDto;
    }
}