using Almostengr.LightShowExtender.DomainService.Common;
using Newtonsoft.Json;

namespace Almostengr.LightShowExtender.Infrastructure.Common;


internal abstract class BaseHttpClient : IBaseHttpClient
{
    internal async Task<T> HttpGetAsync<T>(HttpClient httpClient, string route) where T : BaseDto
    {
        HttpResponseMessage response = await httpClient.GetAsync(route);

        if (response.IsSuccessStatusCode)
        {
            return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
        }

        return null;
    }

    internal string GetUrlWithProtocol(string address)
    {
        if (address.StartsWith("http://") == false && address.StartsWith("https://") == false)
        {
            address = "http://" + address;
        }

        if (address.EndsWith("/") == false)
        {
            address = address + "/";
        }

        return address;
    }
}