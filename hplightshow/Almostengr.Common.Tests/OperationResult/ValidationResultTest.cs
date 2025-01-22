namespace Almostengr.Common.OperationResult.Tests;

public class ValidationResultTest
{
    [Fact]
    public void TestName()
    {
        // Given
        var result = ValidationResult.Create();
        string numberError = "The number is too large";

        // When
        result.AddError(numberError);

        // Then
        Assert.Single(result.Errors);
    }
}