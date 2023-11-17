using Microsoft.Extensions.DependencyInjection;

namespace Almostengr.Common.HomeAssistant.Common;

public static class HomeAssistantFeatureHandlers
{
    public static void AddHandlers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<TurnOnSwitchHandler>();
        serviceCollection.AddSingleton<TurnOffSwitchHandler>();
    }
}
