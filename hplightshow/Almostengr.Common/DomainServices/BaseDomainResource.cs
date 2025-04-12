namespace Almostengr.Common.DomainServices;

public abstract class BaseDomainResource : BaseResource
{
    public Guid Guid { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime ModifiedDate { get; set; }
}
