using Almostengr.LightShowExtender.DomainService.Common;

namespace Almostengr.LightShowExtender.DomainService.NwsWeather;

public sealed class NwsLatestObservationResponseDto : BaseResponseDto
{
    public NwsLatestObservationProperties Properties { get; init; } = new();

    public sealed class NwsLatestObservationProperties
    {
        public NwsLatestObservationTemperature Temperature { get; init; } = new();
        public NwsLatestObservationWindChill WindChill { get; init; } = new();
        public string TextDescription { get; init; } = string.Empty;
        public string TimeStamp { get; init; } = string.Empty;

        public sealed class NwsLatestObservationWindChill
        {
            public string UnitCode { get; init; } = string.Empty;
            public float? Value { get; init; } = null;
        }

        public sealed class NwsLatestObservationTemperature
        {
            public string UnitCode { get; init; } = string.Empty;
            public float? Value { get; init; } = null;
        }
    }
}