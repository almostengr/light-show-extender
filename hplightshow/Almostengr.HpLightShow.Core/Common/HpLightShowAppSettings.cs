using Almostengr.HpLightShow.Core.SequenceSelector.Common;
using Almostengr.HpLightShow.Core.FalconPiPlayer.Common;

namespace Almostengr.HpLightShow.Core.Common;

public sealed class HpLightShowAppSettings
{
    public string ChristmasHashTags { get; init; } = "#Christmas";
    public string IndependenceDayHashTags { get; init; } = "#4thOfJuly";
    public FppAppSettings FppSettings { get; init; } = new();
    public SequenceSelectorAppSettings SequenceSelector { get; init; } = new();
}