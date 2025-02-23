namespace Almostengr.FalconPiPlayerClient.DomainServices.Resources;

public abstract class BaseResource;

public class StatusResource : StatusMessageResource
{
    public int respCode { get; set; }
}

public class StatusMessageResource : BaseResource
{
    public string Message { get; set; }
    public string Status { get; set; }
}