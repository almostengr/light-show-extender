using Almostengr.Common.Domain;
using Microsoft.EntityFrameworkCore;

namespace Almostengr.Common.DomainServices.Interfaces;

public interface IDbContext
{
    DbSet<TEntity> Set<TEntity>() where TEntity : BaseDomainEntity;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

// example of dbocntext implementation with services
// public class UserRepository : DeleteRepository<User>
// {
//     public UserRepository(IDbContext context) : base(context) { }

//     // Add any user-specific repository methods here
// }
