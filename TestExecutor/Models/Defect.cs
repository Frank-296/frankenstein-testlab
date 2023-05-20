namespace TestExecutor.Models;

public class Defect
{
    public String DefectId { get; set; }

    public String ExecutionId { get; set; }

    public String UserId { get; set; }

    public DefectPriority DefectPriority { get; set; }

    public DefectSeverity DefectSeverity { get; set; }

    public String Summary { get; set; }

    public String Description { get; set; }

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