using Almostengr.HpLightShow.Core.FalconPiPlayer.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Almostengr.HpLightShow.Core.FalconPiPlayer.Common;

public sealed class FalconPiPlayerBuilderService
{
    public static void Add(IServiceCollection collection)
    {
        collection.AddTransient<IFppdService, FppdService>();
    }
}