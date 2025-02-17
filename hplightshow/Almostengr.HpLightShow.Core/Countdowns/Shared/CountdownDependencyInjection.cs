using Almostengr.HpLightShow.Core.Countdowns.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Almostengr.HpLightShow.Core.Countdowns.Shared;

public sealed class CountdownDependencyInjection
{
    public static void Add(IServiceCollection collection)
    {
        collection.AddTransient<ICountdownService, CountdownService>();
    }
}
