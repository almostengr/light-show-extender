using System.ComponentModel.DataAnnotations;

namespace Almostengr.Common.Repositories;

public class BaseEntity
{
    [Key]
    public int Id { get; private set; }

    public Guid ReferenceId { get; private set; }

    public DateTime ModifiedDate { get; private set; }

    [Required, MaxLength(100)]
    public string ModifiedBy { get; private set; }

    protected BaseEntity() { }

    public static BaseEntity Create(Guid referenceId)
    {
        return new BaseEntity
        {
            ReferenceId = referenceId
        };
    }

    public void UpdateModifiedBy(string modifiedBy)
    {
        ModifiedDate = DateTime.Now;
        ModifiedBy = modifiedBy;
    }
}
