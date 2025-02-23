using Almostengr.FalconPiPlayerClient.DomainServices.Resources;

namespace Almostengr.FalconPiPlayerClient.Infrastructure;

public sealed class ModelResource : BaseResource
{
    public int ChannelCount { get; set; }
    public string Name { get; set; }
    public string Orientation { get; set; }
    public int StartChannel { get; set; }
    public string StartCorner { get; set; }
    public string StrandsPerString { get; set; }
    public int StringCount { get; set; }
}