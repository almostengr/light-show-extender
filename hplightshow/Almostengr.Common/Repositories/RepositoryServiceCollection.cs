using Almostengr.Common.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Almostengr.Common.Repositories;

public static class RepositoryDependencyInjection
{
    public static void AddServices(IServiceCollection services)
    {
        services.AddTransient(typeof(IAddRepository<>), typeof(AddRepository<>));
        services.AddTransient(typeof(IDeleteRepository<>), typeof(DeleteRepository<>));
        services.AddTransient(typeof(IQueryRepository<>), typeof(QueryRepository<>));
        services.AddTransient(typeof(IUpdateRepository<>), typeof(UpdateRepository<>));
    }
}