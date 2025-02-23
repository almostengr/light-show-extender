using Almostengr.FalconPiPlayerClient.DomainServices.Resources;

namespace Almostengr.FalconPiPlayerClient.Infrastructure;

public sealed class FppdEffectsResource : StatusResource
{
    public List<RunningEffect> RunningEffects { get; set; } = new();

    public class RunningEffect
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
