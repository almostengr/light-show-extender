using Almostengr.Common.Extensions;
using Almostengr.HpLightShow.WebApi.Features.Wled.DomainServices.Interfaces;
using Almostengr.HpLightShow.WebApi.Features.Wled.Infrastructure;

namespace Almostengr.HpLightShow.WebApi.Features.Wled.Shared;

internal sealed class WledDependencyInjection // : IServiceDependencyInjection
{
    public static void AddServices(IServiceCollection services)
    {
        services.AddTransient<IWledClient, WledClient>();
    }
}