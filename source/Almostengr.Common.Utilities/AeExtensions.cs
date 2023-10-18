namespace Almostengr.Common.Utilities;

public static class AeExtensions
{
    public static bool IsNotNullOrWhiteSpace(this string value)
    {
        return !string.IsNullOrWhiteSpace(value);
    }

    public static bool IsNullOrWhiteSpace(this string value)
    {
        return string.IsNullOrWhiteSpace(value);
    }
}