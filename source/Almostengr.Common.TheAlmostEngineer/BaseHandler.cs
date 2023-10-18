using Almostengr.Common.Utilities;
namespace Almostengr.Common.TheAlmostEngineer;

public abstract class BaseHandler
{
    public readonly IBaseHttpClient _httpClient;

    protected BaseHandler(string apiUrl, string apiKey)
    {
        _httpClient = new BaseHttpClient(apiUrl, "X-Auth-Token", apiKey);
    }
}