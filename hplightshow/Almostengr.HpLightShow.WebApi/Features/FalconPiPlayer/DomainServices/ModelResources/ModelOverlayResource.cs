using Almostengr.Common.DomainServices;

namespace Almostengr.FalconPiPlayerClient.Infrastructure;

public sealed class ModelOverlayResource : BaseResource
{
    public List<ModelResource> Models { get; set; } = new();
}