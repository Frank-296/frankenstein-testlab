using TestLab.Utilities;

namespace TestExecutor.Models;

public class Execution
{
    public String ExecutionId { get; set; }

    public String TestCaseId { get; set; }

    public String SuiteExecutionId { get; set; }

    public String UserId { get; set; }

    public DateTime ExecutionDate { get; set; }

    public BrowserDriver BrowserDriver { get; set; }

    public TestEnvironment TestEnvironment { get; set; }

    public Byte[] TestData { get; set; }

    public Byte[] TestReport { get; set; }

    public ExecutionStatus ExecutionStatus { get; set; }

    public String Remarks { get; set; }

    public virtual TestCase TestCase { get; set; }

    public virtual SuiteExecution SuiteExecution { get; set; }

    public virtual Defect Defect { get; set; }

    public virtual User User { get; set; }
}