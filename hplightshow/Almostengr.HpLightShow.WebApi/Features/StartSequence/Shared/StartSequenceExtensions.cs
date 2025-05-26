using Almostengr.HpLightShow.WebApi.Features.StartSequence.DomainServices;
using Almostengr.HpLightShow.WebApi.Features.StartSequence.DomainServices.Interfaces;
using Almostengr.HpLightShow.WebApi.Features.StartSequence.Infrastructure;

namespace Almostengr.HpLightShow.WebApi.Features.StartSequence.Shared;

public static class StartSequenceExtensions
{
    public static void AddStartSequenceServices(this IServiceCollection services)
    {
        services.AddTransient<IFppSequenceRepository, FppSequenceRepository>();
        services.AddTransient<IStartSequenceService, StartSequenceService>();
    }
}
