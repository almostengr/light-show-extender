using Almostengr.Common.Extensions;

namespace Almostengr.Common.Tests.Extensions;

public class TextTests
{
    [Fact]
    public void TestStringNotEmpty()
    {
        string value = "this is stomething";

        bool result = value.IsNotNullOrWhiteSpace();

        Assert.True(result);
    }

    [Fact]
    public void TestStringWhiteSpace()
    {
        string value = "    ";

        bool result = value.IsNotNullOrWhiteSpace();

        Assert.False(result);
    }

    [Fact]
    public void TestStringNotNull()
    {
        string? value = null;

        bool result = value.IsNotNullOrWhiteSpace();

        Assert.False(result);
    }
}