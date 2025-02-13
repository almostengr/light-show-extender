namespace Almostengr.Common.Services;

public abstract class BaseResource
{
    public Guid ReferenceId { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime ModifiedDate { get; set; }
}