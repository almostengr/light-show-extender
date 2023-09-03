using Almostengr.LightShowExtender.DomainService.Common;
using System.Text.Json;
using System.Text;

namespace Almostengr.LightShowExtender.Infrastructure.Common;

internal abstract class BaseHttpClient : IBaseHttpClient
{
    private const string JsonMediaType = "application/json";

    internal async Task<T> HttpGetAsync<T>(HttpClient httpClient, string route) where T : BaseDto
    {
        HttpResponseMessage response = await httpClient.GetAsync(route);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(result);
    }

    internal async Task HttpDeleteAsync(HttpClient httpClient, string route)
    {
        HttpResponseMessage responseMessage = await httpClient.DeleteAsync(route);
        responseMessage.EnsureSuccessStatusCode();
    }

    internal async Task<X> HttpPutAsync<T, X>(HttpClient httpClient, string route, T transferObject) where T : BaseDto where X : BaseDto
    {
        var json = JsonSerializer.Serialize(transferObject);
        var content = new StringContent(json, Encoding.UTF8, JsonMediaType);
        HttpResponseMessage response = await httpClient.PutAsync(route, content);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<X>(result);
    }

    internal async Task<T> HttpPutAsync<T>(HttpClient httpClient, string route, T transferObject) where T : BaseDto
    {
        var json = JsonSerializer.Serialize(transferObject);
        var content = new StringContent(json, Encoding.UTF8, JsonMediaType);
        HttpResponseMessage response = await httpClient.PutAsync(route, content);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(result);
    }

    internal async Task<T> HttpPostAsync<T>(HttpClient httpClient, string route, T transferObject) where T : BaseDto
    {
        var json = JsonSerializer.Serialize(transferObject);
        var content = new StringContent(json, Encoding.UTF8, JsonMediaType);
        HttpResponseMessage response = await httpClient.PostAsync(route, content);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(result);
    }

    internal string GetUrlWithProtocol(string address)
    {
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