using System.Text.Json.Serialization;
using Almostengr.FalconPiPlayerClient.DomainServices.Resources;

namespace Almostengr.FalconPiPlayerClient.DomainServices;

public class StatusResponseResource : StatusMessageResource
{
    [JsonPropertyName("respCode")]
    public int ResponseCode { get; set; }
}
