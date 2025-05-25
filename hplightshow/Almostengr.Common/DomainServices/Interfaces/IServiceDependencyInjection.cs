using Microsoft.Extensions.DependencyInjection;

namespace Almostengr.Common.Extensions;

public interface IServiceDependencyInjection
{
    void AddServices(IServiceCollection services);
}