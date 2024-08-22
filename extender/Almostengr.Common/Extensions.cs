namespace Almostengr.Common;

public static class Extensions
{
    public static string ToFahrenheitFromCelsius(this float? celsius)
    {
        if (celsius == null)
        {
            return "None";
        }

        return ToFahrenheitFromCelsius((float)celsius);
    }

    public static string ToFahrenheitFromCelsius(this float celsius)
    {
        float fahrenheit = (((float)celsius * 9) / 5) + 32;
        int fahrenheitInt = (int)Math.Round(fahrenheit);
        int celsiusInt = (int)Math.Round((float)celsius);

        string output = $"{fahrenheitInt}F ({celsiusInt}C)";
        return output;
    }

    public static bool IsNotNullOrWhiteSpace(this string value)
    {
        return !string.IsNullOrWhiteSpace(value);
    }
}