namespace Almostengr.Common.OperationResult;

public sealed class ServiceResult<TEntity>
{
    private readonly List<string> _errors = new();
    public bool Succeeded => _errors.Count() == 0;
    public bool Failed => !Succeeded;
    public TEntity? Entity { get; private set; }
    public IReadOnlyList<string> Errors => _errors.ToList();

    private ServiceResult(TEntity? entity, IEnumerable<string> errors)
    {
        _ = errors ?? throw new ArgumentNullException(nameof(errors));

        Entity = entity;
        _errors.AddRange(errors);
    }

    public static ServiceResult<TEntity> Success(TEntity entity)
    {
        return new ServiceResult<TEntity>(entity, []);
    }

    public static ServiceResult<TEntity> Failure(Exception exception)
    {
        return new ServiceResult<TEntity>(default, [exception.Message]);
    }

    public static ServiceResult<TEntity> Failure(string error)
    {
        if (string.IsNullOrWhiteSpace(error))
        {
            throw new ArgumentNullException(nameof(error));
        }

        return new ServiceResult<TEntity>(default, [error]);
    }

    public static ServiceResult<TEntity> Failure(IEnumerable<string> errors)
    {
        _ = errors ?? throw new ArgumentNullException(nameof(errors));

        return new ServiceResult<TEntity>(default, errors);
    }

    public static ServiceResult<TEntity> Create()
    {
        return new ServiceResult<TEntity>(default, []);
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
