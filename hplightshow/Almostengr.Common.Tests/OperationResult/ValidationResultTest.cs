namespace Almostengr.Common.OperationResult.Tests;

public class ValidationResultTest
{
    [Fact]
    public void TestName()
    {
        // Given
        var result = new ValidationResult();
        string numberError = "The number is too large";

        // When
        result.AddError(numberError);

        // Then
        Assert.Single(result.Errors);
    }
}