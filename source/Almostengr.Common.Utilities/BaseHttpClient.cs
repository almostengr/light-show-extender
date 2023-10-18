using System.Net;
using System.Text;
using System.Text.Json;

namespace Almostengr.Common.Utilities;

public abstract class BaseHttpClient : IBaseHttpClient
{
    public HttpClient _httpClient;
    
    public async Task<X> HttpDeleteAsync<X>(string route, CancellationToken cancellationToken)
    {
        HttpResponseMessage response = await _httpClient.DeleteAsync(route, cancellationToken);
        await WasRequestSuccessfulAsync(response, cancellationToken);
        return await DeserializeResponseBodyAsync<X>(response);
    }

    public async Task<X> HttpGetAsync<X>(string route, CancellationToken cancellationToken)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(route, cancellationToken);
        await WasRequestSuccessfulAsync(response, cancellationToken);
        string result = await response.Content.ReadAsStringAsync();

        if (typeof(X) == typeof(string))
        {
            return (X)Convert.ChangeType(result, typeof(X));
        }

        return await DeserializeResponseBodyAsync<X>(response);
    }

    public async Task<X> HttpPostAsync<T, X>(string route, T transferObject, CancellationToken cancellationToken) where T : BaseRequestDto where X : BaseResultDto
    {
        StringContent content = SerializeRequestBodyAsync<T>(transferObject);
        HttpResponseMessage response = await _httpClient.PostAsync(route, content, cancellationToken);
        await WasRequestSuccessfulAsync(response, cancellationToken);
        return await DeserializeResponseBodyAsync<X>(response);
    }

    public async Task<X> HttpPutAsync<T, X>(string route, T transferObject, CancellationToken cancellationToken) where T : BaseRequestDto where X : BaseResultDto
    {
        StringContent content = SerializeRequestBodyAsync<T>(transferObject);
        HttpResponseMessage response = await _httpClient.PutAsync(route, content, cancellationToken);
        await WasRequestSuccessfulAsync(response, cancellationToken);
        return await DeserializeResponseBodyAsync<X>(response);
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

    private StringContent SerializeRequestBodyAsync<T>(T transferObject)
    {
        string json = JsonSerializer.Serialize(transferObject);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        return content;
    }

    private async Task<T> DeserializeResponseBodyAsync<T>(HttpResponseMessage response)
    {
        JsonSerializerOptions serializeOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        var result = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(result, serializeOptions)!;
    }

    private async Task WasRequestSuccessfulAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.StatusCode >= HttpStatusCode.InternalServerError ||
            response.StatusCode == HttpStatusCode.RequestTimeout)
        {
            string body = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new ServerErrorException(response.StatusCode, body);
        }

        response.EnsureSuccessStatusCode();
    }

    public sealed class ServerErrorException : Exception
    {
        public ServerErrorException(HttpStatusCode statusCode, string body) :
            base($"Code: {statusCode}, Body: body")
        {
        }
    }
}

public interface IBaseHttpClient
{
    Task<X> HttpDeleteAsync<X>(string route, CancellationToken cancellationToken);
    Task<X> HttpGetAsync<X>(string route, CancellationToken cancellationToken);
    Task<X> HttpPostAsync<T, X>(string route, T transferObject, CancellationToken cancellationToken)
        where T : BaseRequestDto
        where X : BaseResultDto;
    Task<X> HttpPutAsync<T, X>(string route, T transferObject, CancellationToken cancellationToken)
        where T : BaseRequestDto
        where X : BaseResultDto;
}

public abstract class BaseRequestDto
{ }

public abstract class BaseResultDto
{ }
