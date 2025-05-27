using Almostengr.Common.Domain;
using Almostengr.Common.DomainServices;
using Almostengr.Common.DomainServices.Interfaces;
using Almostengr.Common.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Almostengr.Common.Shared;

public static class CommonExtensions
{
    public static void AddCommonServices(this IServiceCollection services)
    {
        services.AddTransient(typeof(IAddRepository<>), typeof(AddRepository<>));
        services.AddTransient(typeof(IDeleteRepository<>), typeof(DeleteRepository<>));
        services.AddTransient(typeof(IQueryRepository<>), typeof(QueryRepository<>));
        services.AddTransient(typeof(IUpdateRepository<>), typeof(UpdateRepository<>));
        services.AddTransient(typeof(IDeleteRepository<>), typeof(DeleteRepository<>));

        services.AddTransient(typeof(IQueryService<,>), typeof(QueryService<,>));
    }

    public static TResource ToResource<TResource, TEntity>(this TEntity entity)
        where TResource : BaseResource, new()
        where TEntity : BaseEntity
    {
        if (entity == null)
        {
            return null;
        }

        return new TResource();
    }
}
