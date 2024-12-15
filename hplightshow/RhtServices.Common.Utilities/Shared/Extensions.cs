namespace RhtServices.Common.Utilities.Shared;

public static class Extensions
{
    public static float ToFahrenheitFromCelsius(this float celsius)
    {
        float fahrenheit = (((float)celsius * 9) / 5) + 32;
        return fahrenheit;
    }

    public static bool IsNotNullOrWhiteSpace(this string value)
    {
        return !string.IsNullOrWhiteSpace(value);
    }
}