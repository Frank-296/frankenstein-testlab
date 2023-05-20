#nullable disable
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api.Models;

[Table("Executions")]
public class Execution
{
    [Key]
    [Required]
    [Column("ExecutionId", TypeName = "uniqueidentifier")]
    public Guid ExecutionId { get; set; }

    [Required]
    public Guid TestCaseId { get; set; }

    public Guid? SuiteExecutionId { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [Required]
    [Display(Name = "Execution date")]
    [Column(TypeName = "smalldatetime")]
    public DateTime ExecutionDate { get; set; }

    [Required]
    [Display(Name = "Browser")]
    public BrowserDriver BrowserDriver { get; set; }

    [Required]
    [Display(Name = "Environment")]
    public TestEnvironment TestEnvironment { get; set; }

    public Byte[] TestData { get; set; }

    [Required]
    public Byte[] TestReport { get; set; }

    [Required]
    [Display(Name = "Status")]
    public ExecutionStatus ExecutionStatus { get; set; }

    public String Remarks { get; set; }

    public virtual TestCase TestCase { get; set; }

    public virtual SuiteExecution SuiteExecution { get; set; }

    public virtual Defect Defect { get; set; }

    public virtual User User { get; set; }
}

public enum TestEnvironment : Int32
{
    [Display(Name = "SIT")]
    Sit = 1,
    [Display(Name = "UAT")]
    Uat = 2,
    [Display(Name = "DEV")]
    Dev = 3,
    [Display(Name = "PROD")]
    Prod = 4
}

public enum ExecutionStatus : Int32
{
    [Display(Name = "Dont exist")]
    DontExist = 0,
    [Display(Name = "Not run")]
    NotRun = 1,
    [Display(Name = "Stop")]
    Stop = 2,
    [Display(Name = "Fail")]
    Fail = 3,
    [Display(Name = "Pass")]
    Pass = 4
}

public enum BrowserDriver : Int32
{
    [Display(Name = "Chrome")]
    Chrome = 1,
    [Display(Name = "Edge")]
    Edge = 2,
    [Display(Name = "Firefox")]
    Firefox = 3,
    [Display(Name = "Safari")]
    Safari = 4
}