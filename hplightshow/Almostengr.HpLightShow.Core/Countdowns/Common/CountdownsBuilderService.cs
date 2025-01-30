using Almostengr.HpLightShow.Core.Countdowns.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Almostengr.HpLightShow.Core.Countdowns.Common;

public sealed class CountdownsBuilderService
{
    public static void Add(IServiceCollection collection)
    {
        collection.AddTransient<ICountdownService, CountdownService>();
    }
}