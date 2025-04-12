using Almostengr.HpLightShow.Core.FalconPiPlayer.DomainServices;
using Almostengr.HpLightShow.Core.FalconPiPlayer.DomainServices.Interfaces;
using Almostengr.HpLightShow.Core.FalconPiPlayer.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Almostengr.HpLightShow.Core.FalconPiPlayer.Shared;

public sealed class FalconPiPlayerDependencyInjection
{
    public static void AddServices(IServiceCollection collection)
    {
        collection.AddSingleton<FppAppSettings>();
        // collection.AddSingleton(configuration.GetSection(nameof(AppSettings)));

        collection.AddTransient<IFppSequenceRepository, FppSequenceRepository>();

        // collection.AddTransient<IFppMonitorService, FppMonitorService>();
        collection.AddTransient<IFppStartSequenceService, FppStartSequenceService>();
    }
}