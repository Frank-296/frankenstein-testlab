namespace TestExecutor.Models;

public class SuiteExecution
{
    public SuiteExecution() => Executions = new HashSet<Execution>();

    public String SuiteExecutionId { get; set; }

    public String BusinessProcessId { get; set; }

    public String UserId { get; set; }

    public DateTime ExecutionDate { get; set; }

    public String Remarks { get; set; }

    public virtual BusinessProcess BusinessProcess { get; set; }

    public virtual User User { get; set; }

    public virtual ICollection<Execution> Executions { get; set; }
}