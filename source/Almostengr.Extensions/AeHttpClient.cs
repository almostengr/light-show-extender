using System.Net;
using System.Text;
using System.Text.Json;

namespace Almostengr.Extensions;

public static class AeHttpClient
{
    public static async Task WasRequestSuccessfulAsync(this HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.StatusCode >= HttpStatusCode.InternalServerError ||
            response.StatusCode == HttpStatusCode.RequestTimeout)
        {
            string body = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new ServerErrorException(response.StatusCode, body);
        }

        response.EnsureSuccessStatusCode();
    }

    public static StringContent SerializeRequestBodyAsync<T>(this T requestObject)
    {
        string json = JsonSerializer.Serialize(requestObject);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        return content;
    }

    public static async Task<T> DeserializeResponseBodyAsync<T>(this HttpResponseMessage response, CancellationToken cancellationToken)
    {
        JsonSerializerOptions serializeOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        var result = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<T>(result, serializeOptions)!;
    }


    public static string GetUrlWithProtocol(this string address)
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
