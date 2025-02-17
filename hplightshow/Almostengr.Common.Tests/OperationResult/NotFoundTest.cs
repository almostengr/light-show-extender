using Almostengr.Common.DomainServices.Results;

namespace Almostengr.Common.Tests;

public class NotFoundTest
{
    [Fact]
    public void TestSuccess()
    {
        Result<string> result = new NotFoundResult<string>();

        Assert.Single(result.Errors);
        Assert.Equal("Not found.", result.Errors.First());
    }
}