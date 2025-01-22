namespace Almostengr.Common.Services;

public abstract class BaseService
{
    public bool IsNotNullOrWhitespace(string value)
    {
        return !string.IsNullOrWhiteSpace(value);
    }
}