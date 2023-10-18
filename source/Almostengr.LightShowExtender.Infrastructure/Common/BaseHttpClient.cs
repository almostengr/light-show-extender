using Almostengr.LightShowExtender.DomainService.Common;
using System.Text.Json;
using System.Text;
using Almostengr.Common.Utilities;

namespace Almostengr.LightShowExtender.Infrastructure.Common;

public abstract class BaseHttpClient : DomainService.Common.IBaseHttpClient
{
    internal async Task<T> HttpGetAsync<T>(HttpClient httpClient, string route)
    {
        HttpResponseMessage response = await httpClient.GetAsync(route);
        await response.WasRequestSuccessfulAsync();
        string result = await response.Content.ReadAsStringAsync();

        if (typeof(T) == typeof(string))
        {
            return (T)Convert.ChangeType(result, typeof(T));
        }

        return await HttpClientUtilities.DeserializeResponseBodyAsync<T>(response);
    }

    internal async Task<X> HttpPutAsync<T, X>(HttpClient httpClient, string route, T transferObject) where T : DomainService.Common.BaseRequestDto where X : BaseResponseDto
    {
        StringContent content = HttpClientUtilities.SerializeRequestBodyAsync<T>(transferObject);
        HttpResponseMessage response = await httpClient.PutAsync(route, content);
        await response.WasRequestSuccessfulAsync();
        return await HttpClientUtilities.DeserializeResponseBodyAsync<X>(response);
    }

    internal async Task<T> HttpPutAsync<T>(HttpClient httpClient, string route, T transferObject) where T : BaseDto
    {
        StringContent content = HttpClientUtilities.SerializeRequestBodyAsync<T>(transferObject);
        HttpResponseMessage response = await httpClient.PutAsync(route, content);
        await response.WasRequestSuccessfulAsync();
        return await HttpClientUtilities.DeserializeResponseBodyAsync<T>(response);
    }

    internal async Task<X> HttpPostAsync<T, X>(HttpClient httpClient, string route, T transferObject) where T : DomainService.Common.BaseRequestDto where X : BaseResponseDto
    {
        StringContent content = HttpClientUtilities.SerializeRequestBodyAsync<T>(transferObject);
        HttpResponseMessage response = await httpClient.PostAsync(route, content);
        await response.WasRequestSuccessfulAsync();
        return await HttpClientUtilities.DeserializeResponseBodyAsync<X>(response);
    }

    internal string GetUrlWithProtocol(string address)
    {
        const string http = "http://";

        if (!address.StartsWith(http) && !address.StartsWith("https://"))
        {
            address = http + address;
        }

        if (!address.EndsWith("/"))
        {
            address = address + "/";
        }

        return address;
    }
}