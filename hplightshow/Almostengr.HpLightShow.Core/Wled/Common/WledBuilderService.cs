using Almostengr.HpLightShow.Core.Wled.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Almostengr.HpLightShow.Core.Wled.Common;

public sealed class WledBuilderService
{
    public static void Add(IServiceCollection collection)
    {
        // collection.AddHttpClient<IWledClient, WledClient>();
    }
}