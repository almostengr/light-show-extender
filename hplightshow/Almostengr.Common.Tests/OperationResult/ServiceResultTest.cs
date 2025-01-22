using Almostengr.Common.OperationResult;

namespace Almostengr.Common.Tests;

public class ServiceResultTest
{
    [Fact]
    public void TestSuccess()
    {
        string testingEnttiy = "entity correct";

        ServiceResult<string> result = ServiceResult<string>.Create();
        result.SetEntity(testingEnttiy);

        Assert.Equal("entity correct", result.Entity);
    }

    [Fact]
    public void TestException()
    {
        Exception exception = new Exception("I just threw an exception");

        ServiceResult<string> result = ServiceResult<string>.Create();
        result.AddError(exception);

        Assert.Single(result.Errors);
    }
}