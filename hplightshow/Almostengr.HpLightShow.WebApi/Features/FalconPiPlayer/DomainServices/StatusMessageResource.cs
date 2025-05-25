using Almostengr.Common.DomainServices;

namespace Almostengr.FalconPiPlayerClient.DomainServices.Resources;

public class StatusMessageResource : BaseResource
{
    public string Message { get; set; }
    public string Status { get; set; }
}