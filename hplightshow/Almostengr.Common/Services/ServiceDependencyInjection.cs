using Almostengr.Common.Services;
using Almostengr.Common.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Almostengr.Common.services;

public static class ServiceDependencyInjection
{
    public static void AddServices(IServiceCollection services)
    {
        services.AddTransient(typeof(IQueryService<,>), typeof(QueryService<,>));
    }
}