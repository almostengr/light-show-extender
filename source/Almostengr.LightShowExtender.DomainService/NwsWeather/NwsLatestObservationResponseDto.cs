using Almostengr.LightShowExtender.DomainService.Common;

namespace Almostengr.LightShowExtender.DomainService.NwsWeather;

public sealed class NwsLatestObservationResponseDto : BaseResponseDto
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