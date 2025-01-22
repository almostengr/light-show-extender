namespace Almostengr.HpLightShow.Core.SequenceSelector.Enums;

internal sealed class LightingSequence
{
    public LightingSequence(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static readonly LightingSequence Blue = new("Calendar Blue.fseq");
    public static readonly LightingSequence BlueGreen = new("Calendar Blue Green.fseq");
    public static readonly LightingSequence ChristmasShow = new("Christmas.json");
    public static readonly LightingSequence Green = new("Calendar Green.fseq");
    public static readonly LightingSequence IndependenceDayShow = new("Independence.json");
    public static readonly LightingSequence Pink = new("Calendar Pink.fseq");
    public static readonly LightingSequence PinkCyanYellow = new("Calendar Pink Cyan Yellow.fseq");
    public static readonly LightingSequence Purple = new("Calendar Purple.fseq");
    public static readonly LightingSequence Red = new("Calendar Red.fseq");
    public static readonly LightingSequence RedBlue = new("Calendar Red Blue.fseq");
    public static readonly LightingSequence RedGreen = new("Calendar Red Green.fseq");
    public static readonly LightingSequence RedGreenBlack = new("Calendar Red Green Black.fseq");
    public static readonly LightingSequence RedPink = new("Calendar Red Pink.fseq");
    public static readonly LightingSequence Yellow = new("Calendar Yellow.fseq");
}