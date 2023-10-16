using System.Text;
using System.Text.Json;

namespace Almostengr.Common.Utilities;
public static class HttpClientUtilities
{
    public static StringContent SerializeRequestBodyAsync<T>(T transferObject)
    {
        string json = JsonSerializer.Serialize(transferObject);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        return content;
    }

    public static async Task<T> DeserializeResponseBodyAsync<T>(HttpResponseMessage response)
    {
        JsonSerializerOptions serializeOptions =  new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        var result = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(result, serializeOptions)!;
    }
}
