namespace Almostengr.Common.Repositories.Interfaces;

public interface IAddRepository<TEntity> : IQueryRepository<TEntity> where TEntity : BaseEntity
{
    Task AddAsync(TEntity entity);
    Task CommitAsync();
}

// public class UserRepository : DeleteRepository<User>
// {
//     public UserRepository(IDbContext context) : base(context) { }

//     // Add any user-specific repository methods here
// }
