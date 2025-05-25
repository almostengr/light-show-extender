
namespace Almostengr.FalconPiPlayerClient.Infrastructure;

public interface IModelHttpClient
{
    Task<List<ModelResource>> GetModelsAsync();
    Task<string> PostModelOverlaysAsync(ModelOverlayResource resource);
    Task<ModelResource> GetModelByNameAsync(string name);
}