using System.Text;
using System.Text.Json;
using Almostengr.FalconPiPlayerClient.DomainServices.Resources;

namespace Almostengr.FalconPiPlayerClient.Infrastructure;

public abstract class BaseClient
{
    protected readonly HttpClient _httpClient;

    public BaseClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    protected StringContent SerializeRequestBody<TResource>(TResource resource) where TResource : BaseResource
    {
        ArgumentNullException.ThrowIfNull(resource, nameof(resource));

        string json = JsonSerializer.Serialize(resource);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        return content;
    }

    protected async Task<TResource> DeserializeResponseBodyAsync<TResource>(HttpResponseMessage response) where TResource : class
    {
        ArgumentNullException.ThrowIfNull(response, nameof(response));

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();

        JsonSerializerOptions serializeOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        return JsonSerializer.Deserialize<TResource>(result, serializeOptions)!;
    }
}
