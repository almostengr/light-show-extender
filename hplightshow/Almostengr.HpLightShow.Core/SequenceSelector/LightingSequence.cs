namespace Almostengr.HpLightShow.Core.DomainHandler.SequenceSelector;

internal sealed class LightingSequence
{
    public LightingSequence(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static readonly LightingSequence Blue = new LightingSequence("Calendar Blue.fseq");
    public static readonly LightingSequence BlueGreen = new LightingSequence("Calendar Blue Green.fseq");
    public static readonly LightingSequence ChristmasShow = new LightingSequence("Christmas.json");
    public static readonly LightingSequence Green = new LightingSequence("Calendar Green.fseq");
    public static readonly LightingSequence IndependenceDayShow = new LightingSequence("Independence.json");
    public static readonly LightingSequence Pink = new LightingSequence("Calendar Pink.fseq");
    public static readonly LightingSequence PinkCyanYellow = new LightingSequence("Calendar Pink Cyan Yellow.fseq");
    public static readonly LightingSequence Purple = new LightingSequence("Calendar Purple.fseq");
    public static readonly LightingSequence Red = new LightingSequence("Calendar Red.fseq");
    public static readonly LightingSequence RedBlue = new LightingSequence("Calendar Red Blue.fseq");
    public static readonly LightingSequence RedGreen = new LightingSequence("Calendar Red Green.fseq");
    public static readonly LightingSequence RedGreenBlack = new LightingSequence("Calendar Red Green Black.fseq");
    public static readonly LightingSequence RedPink = new LightingSequence("Calendar Red Pink.fseq");
    public static readonly LightingSequence Yellow = new LightingSequence("Calendar Yellow.fseq");
}