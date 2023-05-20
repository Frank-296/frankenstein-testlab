#nullable disable
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api.Models;

[Table("SuiteExecutions")]
public class SuiteExecution
{
    public SuiteExecution() => Executions = new HashSet<Execution>();

    [Key]
    [Required]
    [Column("SuiteExecutionId", TypeName = "uniqueidentifier")]
    public Guid SuiteExecutionId { get; set; }

    [Required]
    public Guid BusinessProcessId { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [Required]
    [Display(Name = "Execution date")]
    [Column(TypeName = "smalldatetime")]
    public DateTime ExecutionDate { get; set; }

    public String Remarks { get; set; }

    public virtual BusinessProcess BusinessProcess { get; set; }

    public virtual User User { get; set; }

    public virtual ICollection<Execution> Executions { get; set; }
}