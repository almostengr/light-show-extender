using Almostengr.LightShowExtender.DomainService.Common;
using Newtonsoft.Json;

namespace Almostengr.LightShowExtender.Infrastructure.Common;

internal abstract class BaseHttpClient : IBaseHttpClient
{
    internal async Task<T> HttpGetAsync<T>(HttpClient httpClient, string route) where T : BaseDto
    {
        HttpResponseMessage response = await httpClient.GetAsync(route);
        response.EnsureSuccessStatusCode();
        return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
    }

    internal string GetUrlWithProtocol(string address)
    {
        address = address.ToLower();

        if (!address.StartsWith("http://") && !address.StartsWith("https://"))
        {
            address = "http://" + address;
        }

        if (!address.EndsWith("/"))
        {
            address = address + "/";
        }

        return address;
    }
}