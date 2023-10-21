namespace Almostengr.LightShowExtender.DomainService.Common;

public static class Extensions
{

    public static bool IsNotNullOrWhiteSpace(this string input)
    {
        return !string.IsNullOrWhiteSpace(input);
    }

    public static bool IsNullOrWhiteSpace(this string input)
    {
        return string.IsNullOrWhiteSpace(input);
    }
}