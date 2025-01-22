namespace Almostengr.Common.OperationResult;

public sealed class ServiceResult<TEntity>
{
    private readonly List<string> _errors = new();
    public bool Succeeded => _errors.Count() == 0;
    public bool Failed => !Succeeded;
    public TEntity? Entity { get; private set; }
    public IReadOnlyList<string> Errors => _errors.AsReadOnly();

    private ServiceResult() { }

    public static ServiceResult<TEntity> Create()
    {
        return new ServiceResult<TEntity>();
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

    public void SetEntity(TEntity entity)
    {
        _ = entity ?? throw new ArgumentNullException(nameof(entity));

        Entity = entity;
    }
}
