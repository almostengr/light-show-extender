namespace Almostengr.Common.OperationResult;

public sealed class ValidationResult
{
    private readonly List<string> _errors = new();
    public bool IsValid => _errors.Count() == 0;
    public bool NotValid => _errors.Count() > 0;
    public IReadOnlyList<string> Errors => _errors.ToList();

    public ValidationResult()
    { }

    public void AddError(string error)
    {
        if (string.IsNullOrWhiteSpace(error))
        {
            throw new ArgumentNullException(nameof(error));
        }

        _errors.Add(error);
    }

    public void AddErrors(IEnumerable<string> errors)
    {
        _  = errors ?? throw new ArgumentNullException(nameof(errors));

        _errors.AddRange(errors);
    }
}
