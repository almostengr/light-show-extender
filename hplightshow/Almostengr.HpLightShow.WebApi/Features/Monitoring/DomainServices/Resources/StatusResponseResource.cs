using System.Text.Json.Serialization;

namespace Almostengr.HpLightShow.WebApi.Features.Monitoring.DomainServices.Resources;

public class StatusResponseResource : StatusMessageResource
{
    [JsonPropertyName("respCode")]
    public int ResponseCode { get; set; }
}
