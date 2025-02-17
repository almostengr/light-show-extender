using System.Net;
using System.Text;
using System.Text.Json;
using Almostengr.Common.DomainServices;

namespace Almostengr.Common.Infrastructure;

public static class HttpClientExtensions
{
    private static async Task WasRequestSuccessfulAsync(this HttpResponseMessage response)
    {
        if (response.StatusCode >= HttpStatusCode.InternalServerError ||
            response.StatusCode == HttpStatusCode.RequestTimeout)
        {
            string body = await response.Content.ReadAsStringAsync();
            throw new ServerErrorException(response.StatusCode, body);
        }

        response.EnsureSuccessStatusCode();
    }

    private static StringContent SerializeRequestBody<TResource>(this TResource request) where TResource : BaseResource
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        string json = JsonSerializer.Serialize(request);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        return content;
    }

    private static async Task<TResource> DeserializeResponseBodyAsync<TResource>(this HttpResponseMessage response) where TResource : BaseResource
    {
        _ = response ?? throw new ArgumentNullException(nameof(response));

        var result = await response.Content.ReadAsStringAsync();

        JsonSerializerOptions serializeOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        return JsonSerializer.Deserialize<TResource>(result, serializeOptions)!;
    }

    public static string GetUrlWithProtocol(this string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentNullException(nameof(url));
        }

        if (url.ToLower().StartsWith("http"))
        {
            return url;
        }

        url = url.EndsWith("/") ? url.Substring(0, url.Length - 1) : url;

        return "http://" + url;
    }

    public static async Task<string> GetStringAsync<TResource>(this HttpClient httpClient, string route)where TResource : BaseResource
    {
        _ = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _ = route ?? throw new ArgumentNullException(nameof(route));

        var response = await httpClient.GetAsync(route);
        await response.WasRequestSuccessfulAsync();
        return await response.Content.ReadAsStringAsync();
    }

    public static async Task<TResource> GetAsync<TResource>(this HttpClient httpClient, string route) where TResource : BaseResource
    {
        _ = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _ = route ?? throw new ArgumentNullException(nameof(route));

        var response = await httpClient.GetAsync(route);
        await response.WasRequestSuccessfulAsync();
        return await response.DeserializeResponseBodyAsync<TResource>();
    }

    public static async Task<XResource> PostAsync<TResource, XResource>(this HttpClient httpClient, string route, TResource request) where TResource : BaseResource where XResource : BaseResource
    {
        _ = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _ = route ?? throw new ArgumentNullException(nameof(route));
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var serializedRequest = request.SerializeRequestBody<TResource>();
        var response = await httpClient.PostAsync(route, serializedRequest);
        await response.WasRequestSuccessfulAsync();
        return await response.DeserializeResponseBodyAsync<XResource>();
    }

    public static async Task<XResource> PutAsync<TResource, XResource>(this HttpClient httpClient, string route, TResource request) where TResource : BaseResource where XResource : BaseResource
    {
        _ = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _ = route ?? throw new ArgumentNullException(nameof(route));
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var serializedRequest = request.SerializeRequestBody<TResource>();
        var response = await httpClient.PutAsync(route, serializedRequest);
        await response.WasRequestSuccessfulAsync();
        return await response.DeserializeResponseBodyAsync<XResource>();
    }

    public static async Task DeleteAsync(this HttpClient httpClient, string route)
    {
        _ = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _ = route ?? throw new ArgumentNullException(nameof(route));

        var response = await httpClient.DeleteAsync(route);
        await response.WasRequestSuccessfulAsync();
    }
}
