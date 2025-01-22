using Almostengr.Common.OperationResult;

namespace Almostengr.Common.Tests;

public class ServiceResultTest
{
    [Fact]
    public void TestSuccess()
    {
        string testingEnttiy = "entity correct";

        ServiceResult<string> ServiceResult = ServiceResult<string>.Success(testingEnttiy);

        Assert.Equal("entity correct", ServiceResult.Entity);
    }

    [Fact]
    public void TestException()
    {
        Exception exception = new Exception("I just threw an exception");

        ServiceResult<string> ServiceResult = ServiceResult<string>.Failure(exception);

        Assert.Single(ServiceResult.Errors);
    }
}