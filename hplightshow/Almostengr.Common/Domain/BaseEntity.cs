using System.ComponentModel.DataAnnotations;
using Almostengr.Common.DomainServices.Results;

namespace Almostengr.Common.Domain;

public abstract class BaseEntity
{
    [Key]
    public int Id { get; private set; }

    [Required]
    public Guid Guid { get; private set; }

    public DateTime ModifiedDate { get; private set; }

    [Required, MaxLength(100)]
    public string ModifiedBy { get; private set; }

    protected Result<BaseEntity> SetModified(string modifiedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(modifiedBy, nameof(modifiedBy));

        ModifiedDate = DateTime.Now;
        ModifiedBy = modifiedBy;

        return Result<BaseEntity>.Success(this);
    }
}
