using Almostengr.FalconPiPlayerClient.DomainServices;

namespace Almostengr.FalconPiPlayerClient.Infrastructure;

public sealed class FppdEffectsResource : StatusResponseResource
{
    public List<RunningEffect> RunningEffects { get; set; } = new();

    public class RunningEffect
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
