namespace Almostengr.Common.OperationResult;

public sealed class ValidationResult
{
    private readonly List<string> _errors = new();
    public bool IsValid => _errors.Count() == 0;
    public bool NotValid => !IsValid;
    public IReadOnlyList<string> Errors => _errors.ToList();

    private ValidationResult()
    { }

    public static ValidationResult Create()
    {
        return new ValidationResult();
    }

    public void AddError(string error)
    {
        if (string.IsNullOrWhiteSpace(error))
        {
            throw new ArgumentNullException(nameof(error));
        }

        _errors.Add(error);
    }

    public void AddError(Exception exception)
    {
        _ = exception ?? throw new ArgumentNullException(nameof(exception));

        _errors.Add(exception.Message);
    }

    public void AddErrors(IEnumerable<string> errors)
    {
        _ = errors ?? throw new ArgumentNullException(nameof(errors));

        _errors.AddRange(errors);
    }
}
