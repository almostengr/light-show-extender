using System.Net;
using System.Text;
using System.Text.Json;
using RhtServices.Common.Utilities.Exceptions;

namespace RhtServices.Common.Utilities.Infrastructure;

public static class AeHttpClient
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

    public static StringContent SerializeRequestBody<T>(this T request)
    {
        string json = JsonSerializer.Serialize(request);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        return content;
    }

    private static async Task<T> DeserializeResponseBodyAsync<T>(this HttpResponseMessage response)
    {
        var result = await response.Content.ReadAsStringAsync();

        JsonSerializerOptions serializeOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        return JsonSerializer.Deserialize<T>(result, serializeOptions)!;
    }

    public static string GetUrlWithProtocol(this string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentNullException("Invalid url provided.");
        }

        if (url.ToLower().StartsWith("http"))
        {
            return url;
        }

        url = url.EndsWith("/") ? url.Substring(0, url.Length - 1) : url;

        return "http://" + url;
    }

    public static async Task<string> GetStringAsync<T>(this HttpClient httpClient, string route)
    {
        var response = await httpClient.GetAsync(route);
        await response.WasRequestSuccessfulAsync();
        return await response.Content.ReadAsStringAsync();
    }

    public static async Task<T> GetAsync<T>(this HttpClient httpClient, string route)
    {
        var response = await httpClient.GetAsync(route);
        await response.WasRequestSuccessfulAsync();
        return await response.DeserializeResponseBodyAsync<T>();
    }

    public static async Task<X> PostAsync<T, X>(this HttpClient httpClient, string route, T request)
    {
        var serializedRequest = request.SerializeRequestBody<T>();
        var response = await httpClient.PostAsync(route, serializedRequest);
        await response.WasRequestSuccessfulAsync();
        return await response.DeserializeResponseBodyAsync<X>();
    }

    public static async Task<X> PutAsync<T, X>(this HttpClient httpClient, string route, T request)
    {
        var serializedRequest = request.SerializeRequestBody<T>();
        var response = await httpClient.PutAsync(route, serializedRequest);
        await response.WasRequestSuccessfulAsync();
        return await response.DeserializeResponseBodyAsync<X>();
    }

    public static async Task DeleteAsync(this HttpClient httpClient, string route)
    {
        var response = await httpClient.DeleteAsync(route);
        await response.WasRequestSuccessfulAsync();
    }
}
