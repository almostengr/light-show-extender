using Almostengr.Extensions;

namespace Almostengr.NationalWeatherService.DomainService;

public sealed class NwsLatestObservationResponse : IQueryResponse
{
    public NwsLatestObservationProperties Properties { get; init; } = new();

    public sealed class NwsLatestObservationProperties
    {
        public NwsLatestObservationTemperature Temperature { get; init; } = new();
        public NwsLatestObservationWindChill WindChill { get; init; } = new();

        public sealed class NwsLatestObservationWindChill
        {
            public float? Value { get; init; } = null;
        }

        public sealed class NwsLatestObservationTemperature
        {
            public float? Value { get; init; } = null;
        }
    }
}
