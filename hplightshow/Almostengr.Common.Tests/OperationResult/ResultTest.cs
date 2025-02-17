using Almostengr.Common.DomainServices.Results;

namespace Almostengr.Common.Tests;

public class ResultTest
{
    [Fact]
    public void TestSuccess()
    {
        string testingEnttiy = "entity correct";

        Result<string> result = Result<string>.Create();
        result.SetValue(testingEnttiy);

        Assert.Equal("entity correct", result.Value);
    }

    [Fact]
    public void TestException()
    {
        Exception exception = new Exception("I just threw an exception");

        Result<string> result = Result<string>.Create();
        result.AddError(exception);

        Assert.Single(result.Errors);
    }


    [Fact]
    public void TestName()
    {
        // Given
        var result = Result<string>.Create();
        string numberError = "The number is too large";

        // When
        result.AddError(numberError);

        // Then
        Assert.Single(result.Errors);
    }
}