using System.Net;
using System.Text;
using System.Text.Json;
using Almostengr.Common.Utilities.Exceptions;

namespace Almostengr.Common.Utilities.Infrastructure;

public static class AeHttpClient
{
    private static async Task WasRequestSuccessfulAsync(this HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.StatusCode >= HttpStatusCode.InternalServerError ||
            response.StatusCode == HttpStatusCode.RequestTimeout)
        {
            string body = await response.Content.ReadAsStringAsync(cancellationToken);
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

    private static async Task<T> DeserializeResponseBodyAsync<T>(this HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var result = await response.Content.ReadAsStringAsync(cancellationToken);

        JsonSerializerOptions serializeOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        return JsonSerializer.Deserialize<T>(result, serializeOptions)!;
    }

    public static string GetUrlWithProtocol(this string address)
    {
        const string HTTP = "http://";

        if (!address.StartsWith(HTTP) && !address.StartsWith("https://"))
        {
            address = HTTP + address;
        }

        if (!address.EndsWith("/"))
        {
            address = address + "/";
        }

        return address;
    }

    public static async Task<string> GetStringAsync<T>(this HttpClient httpClient, string route, CancellationToken cancellationToken)
    {
        var response = await httpClient.GetAsync(route, cancellationToken);
        await response.WasRequestSuccessfulAsync(cancellationToken);
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    public static async Task<T> GetAsync<T>(this HttpClient httpClient, string route, CancellationToken cancellationToken)
    {
        var response = await httpClient.GetAsync(route, cancellationToken);
        await response.WasRequestSuccessfulAsync(cancellationToken);
        return await response.DeserializeResponseBodyAsync<T>(cancellationToken);
    }

    public static async Task<X> PostAsync<T, X>(this HttpClient httpClient, string route, T request, CancellationToken cancellationToken)
    {
        var serializedRequest = request.SerializeRequestBody<T>();
        var response = await httpClient.PostAsync(route, serializedRequest, cancellationToken);
        await response.WasRequestSuccessfulAsync(cancellationToken);
        return await response.DeserializeResponseBodyAsync<X>(cancellationToken);
    }

    public static async Task<X> PutAsync<T, X>(this HttpClient httpClient, string route, T request, CancellationToken cancellationToken)
    {
        var serializedRequest = request.SerializeRequestBody<T>();
        var response = await httpClient.PutAsync(route, serializedRequest, cancellationToken);
        await response.WasRequestSuccessfulAsync(cancellationToken);
        return await response.DeserializeResponseBodyAsync<X>(cancellationToken);
    }

    public static async Task DeleteAsync(this HttpClient httpClient, string route, CancellationToken cancellationToken)
    {
        var response = await httpClient.DeleteAsync(route, cancellationToken);
        await response.WasRequestSuccessfulAsync(cancellationToken);
    }
}
