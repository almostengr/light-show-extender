using System.ComponentModel.DataAnnotations;

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

    public BaseEntity() { }

    public BaseEntity(string modifiedBy) : this()
    {
        UpdateModifiedBy(modifiedBy);
    }

    public void UpdateModifiedBy(string modifiedBy)
    {
        ModifiedDate = DateTime.Now;
        ModifiedBy = modifiedBy;
    }
}
