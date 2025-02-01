namespace Almostengr.HpLightShow.Core.Wled.Resources;

public sealed class WledStatusResource
{
    public WledState State { get; set; } = new();


    public sealed class WledState
    {
        public bool On { get; set; }
    }
}
