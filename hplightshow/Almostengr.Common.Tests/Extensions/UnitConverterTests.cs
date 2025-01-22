using Almostengr.Common.Extensions;

namespace Almostengr.Common.Tests.Extensions;

public class UnitConverterTests
{
    public void ToFahrenheitTest()
    {
        float celsius = 0;

        float fahrenheit = celsius.ToFahrenheit();

        Assert.Equal(32, fahrenheit);
    }

    public void ToCelsiusTest()
    {
        float fahrenheit = 32;

        float celsius = fahrenheit.ToCelsius();

        Assert.Equal(0, celsius);
    }
}