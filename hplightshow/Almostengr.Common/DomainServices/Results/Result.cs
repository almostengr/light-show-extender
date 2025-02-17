namespace Almostengr.Common.DomainServices.Results;

public class Result<TValue>
{
    private readonly List<string> _errors = new();
    public bool Succeeded => _errors.Count() == 0;
    public bool Failed => !Succeeded;
    public TValue Value { get; private set; }
    public IReadOnlyList<string> Errors => _errors.AsReadOnly();

    protected Result(TValue value, IEnumerable<string> errors)
    {
        Value = value;

        if (errors != null)
        {
            _errors.AddRange(errors);
        }
    }

    public static Result<TValue> Create()
    {
        return new Result<TValue>(default, null);
    }

    public static Result<TValue> Success(TValue value)
    {
        return new Result<TValue>(value, null);
    }

    public static Result<TValue> Failure(string error)
    {
        return new Result<TValue>(default, [error]);
    }

    public static Result<TValue> Failure(Exception exception)
    {
        return new Result<TValue>(default, [exception.Message]);
    }

    public static Result<TValue> Failure(IEnumerable<string> errors)
    {
        return new Result<TValue>(default, errors);
    }

    public void AddError(Exception exception)
    {
        _ = exception ?? throw new ArgumentNullException(nameof(exception));

        _errors.Add(exception.Message);
    }

    public void AddError(string error)
    {
        if (string.IsNullOrEmpty(error))
        {
            throw new ArgumentNullException(nameof(error));
        }

        _errors.Add(error);
    }

    public void AddErrors(IEnumerable<string> errors)
    {
        _errors.AddRange(errors);
    }

    public void SetValue(TValue value)
    {
        _ = value ?? throw new ArgumentNullException(nameof(value));

        Value = value;
    }
}
