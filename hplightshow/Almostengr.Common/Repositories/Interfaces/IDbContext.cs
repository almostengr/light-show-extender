using Microsoft.EntityFrameworkCore;

namespace Almostengr.Common.Repositories.Interfaces;

public interface IDbContext
{
    DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
