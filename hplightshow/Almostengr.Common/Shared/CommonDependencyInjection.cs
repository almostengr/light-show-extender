using Almostengr.Common.DomainServices;
using Almostengr.Common.DomainServices.Interfaces;
using Almostengr.Common.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Almostengr.Common.Extensions;

public static class CommonDependencyInjection
{
    public static void AddServices(IServiceCollection services)
    {
        services.AddTransient(typeof(IAddRepository<>), typeof(AddRepository<>));
        services.AddTransient(typeof(IDeleteRepository<>), typeof(DeleteRepository<>));
        services.AddTransient(typeof(IQueryRepository<>), typeof(QueryRepository<>));
        services.AddTransient(typeof(IUpdateRepository<>), typeof(UpdateRepository<>));
        
        services.AddTransient(typeof(IQueryService<,>), typeof(QueryService<,>));
    }
}