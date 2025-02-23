using Almostengr.FalconPiPlayerClient.DomainServices.Resources;

namespace Almostengr.FalconPiPlayerClient.Infrastructure;

public sealed class ModelOverlayResource : BaseResource
{
    public List<ModelResource> Models { get; set; } = new();
}