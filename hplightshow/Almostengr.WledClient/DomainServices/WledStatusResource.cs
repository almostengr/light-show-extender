using Almostengr.Common.DomainServices;

namespace Almostengr.WledClient.DomainServices;

public sealed class WledStatusResource : BaseResource
{
    public WledState State { get; set; } = new();


    public sealed class WledState
    {
        public bool On { get; set; }
    }
}
