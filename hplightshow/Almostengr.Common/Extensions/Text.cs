namespace Almostengr.Common.Extensions;

public static class Text
{
    public static bool IsNotNullOrWhiteSpace(this string value)
    {
        return !string.IsNullOrWhiteSpace(value);
    }
}