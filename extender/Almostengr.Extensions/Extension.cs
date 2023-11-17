namespace Almostengr.LightShowExtender.DomainService.Common;

public static class Extensions
{
    public static string ToDisplayTemperature(this float? celsius)
    {
        if (celsius == null)
        {
            return "None";
        }

        return ToDisplayTemperature((float)celsius);
    }

    public static string ToDisplayTemperature(this float celsius)
    {
        float fahrenheit = (((float)celsius * 9) / 5) + 32;
        int fahrenheitInt = (int)Math.Round(fahrenheit);
        int celsiusInt = (int)Math.Round((float)celsius);

        string output = $"{fahrenheitInt}F ({celsiusInt}C)";
        return output;
    }
}