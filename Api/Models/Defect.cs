#nullable disable
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api.Models;

[Table("Defects")]
public class Defect
{
    [Key]
    [Required]
    [Column("DefectId", TypeName = "uniqueidentifier")]
    public Guid DefectId { get; set; }

    [Required]
    public Guid ExecutionId { get; set; }

    public Guid? UserId { get; set; }

    [Required]
    [Display(Name = "Defect priority")]
    public DefectPriority DefectPriority { get; set; }

    [Required]
    [Display(Name = "Defect severity")]
    public DefectSeverity DefectSeverity { get; set; }

    [Required]
    [Display(Name = "Summary")]
    public String Summary { get; set; }

    [Required]
    [Display(Name = "Defect description")]
    public String Description { get; set; }

    [Required]
    [Display(Name = "Defect status")]
    public DefectStatus DefectStatus { get; set; }

    public virtual Execution Execution { get; set; }

    public virtual User User { get; set; }
}

public enum DefectPriority : Int32
{
    Low = 1,
    Medium = 2,
    High = 3
}

public enum DefectSeverity : Int32
{
    Cosmetic = 1,
    Low = 2,
    Medium = 3,
    High = 4,
    Critical = 5
}

public enum DefectStatus : Int32
{
    New = 1,
    Assigned = 2,
    Open = 3,
    Duplicated = 4,
    Rejected = 5,
    Deferred = 6,
    NotABug = 7,
    Fixed = 8,
    Retested = 9,
    Reopened = 10,
    Closed = 11
}