using System.ComponentModel.DataAnnotations;
using Almostengr.Common.DomainServices.Results;

namespace Almostengr.Common.Domain;

public abstract class BaseDomainEntity : BaseEntity
{
    [Key]
    public int Id { get; private set; }

    [Required]
    public Guid Guid { get; private set; }

    public DateTime ModifiedDate { get; private set; }

    [Required, MaxLength(100)]
    public string ModifiedBy { get; private set; }

    protected Result<BaseDomainEntity> SetModified(string modifiedBy)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(modifiedBy, nameof(modifiedBy));

        ModifiedDate = DateTime.Now;
        ModifiedBy = modifiedBy;

        return Result<BaseDomainEntity>.Success(this);
    }
}
