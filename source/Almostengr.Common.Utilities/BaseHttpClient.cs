using System.Text;
using System.Text.Json;

namespace Almostengr.Common.Utilities;

// public static class HttpClientUtilities
// {
//     public static StringContent SerializeRequestBodyAsync<T>(T transferObject)
//     {
//         string json = JsonSerializer.Serialize(transferObject);
//         StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
//         return content;
//     }

//     public static async Task<T> DeserializeResponseBodyAsync<T>(HttpResponseMessage response)
//     {
//         JsonSerializerOptions serializeOptions =  new JsonSerializerOptions
//         {
//             PropertyNameCaseInsensitive = true,
//         };

//         var result = await response.Content.ReadAsStringAsync();
//         return JsonSerializer.Deserialize<T>(result, serializeOptions)!;
//     }
// }

public abstract class BaseHttpClient
{
    private readonly HttpClient _httpClient;

    public BaseHttpClient(string baseAddress, string apiKeyType = null, string apiKeyValue = null)
    {
        if (string.IsNullOrWhiteSpace(baseAddress))
        {
            throw new ArgumentNullException(nameof(baseAddress));
        }

        _httpClient = new HttpClient();

        if (apiKeyType.IsNotNullOrWhiteSpace() && apiKeyValue.IsNotNullOrWhiteSpace())
        {
            _httpClient.DefaultRequestHeaders.Clear();
        }

        _httpClient.BaseAddress = new Uri(baseAddress);
    }

    public async Task<X> HttpDeleteAsync<X>(string route) where X : BaseResponseDto
    {
        HttpResponseMessage response = await _httpClient.DeleteAsync(route);
        await response.WasRequestSuccessfulAsync();
        return await DeserializeResponseBodyAsync<X>(response);
    }

    public async Task<X> HttpGetAsync<X>(string route)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(route);
        await response.WasRequestSuccessfulAsync();
        string result = await response.Content.ReadAsStringAsync();

        if (typeof(X) == typeof(string))
        {
            return (X)Convert.ChangeType(result, typeof(X));
        }

        return await HttpClientUtilities.DeserializeResponseBodyAsync<T>(response);
    }

    public async Task<X> HttpPostAsync<T, X>(string route, T transferObject) where T : BaseRequestDto where X : BaseResponseDto
    {
        StringContent content = SerializeRequestBodyAsync<T>(transferObject);
        HttpResponseMessage response = await _httpClient.PostAsync(route, content);
        await response.WasRequestSuccessfulAsync();
        return await DeserializeResponseBodyAsync<X>(response);
    }

    public async Task<X> HttpPutAsync<T, X>(string route, T transferObject) where T : BaseRequestDto where X : BaseResponseDto
    {
        StringContent content = HttpClientUtilities.SerializeRequestBodyAsync<T>(transferObject);
        HttpResponseMessage response = await _httpClient.PutAsync(route, content);
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

    private StringContent SerializeRequestBodyAsync<T>(T transferObject)
    {
        string json = JsonSerializer.Serialize(transferObject);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        return content;
    }

    private async Task<T> DeserializeResponseBodyAsync<T>(HttpResponseMessage response)
    {
        JsonSerializerOptions serializeOptions =  new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        var result = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(result, serializeOptions)!;
    }

    private static async Task<HttpResponseMessage> WasRequestSuccessfulAsync(this HttpResponseMessage response)
    {
        if (response.StatusCode >= HttpStatusCode.InternalServerError ||
            response.StatusCode == HttpStatusCode.RequestTimeout)
        {
            string body = await response.Content.ReadAsStringAsync()!;
            throw new ServerErrorException(response.StatusCode, body);
        }

        response.EnsureSuccessStatusCode();
        return response;
    }
}

public abstract class BaseRequestDto
{}

public abstract class BaseResponseDto
{}
