namespace Almostengr.FalconPiPlayerClient.Infrastructure;

public sealed class ModelHttpClient : BaseClient, IModelHttpClient
{
    public ModelHttpClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<List<ModelResource>> GetModelsAsync()
    {
        var response = await _httpClient.GetAsync("api/models");
        var result = await DeserializeResponseBodyAsync<List<ModelResource>>(response);
        return result;
    }

    public async Task<string> PostModelOverlaysAsync(ModelOverlayResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource, nameof(resource));

        var json = SerializeRequestBody<ModelOverlayResource>(resource);
        var response = await _httpClient.PostAsync("api/models", json);
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }

    public async Task<ModelResource> GetModelByNameAsync(string name)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(name);

        var route = $"api/models/{name}";
        var response = await _httpClient.GetAsync(route);
        var result = await DeserializeResponseBodyAsync<ModelResource>(response);
        return result;
    }
}
