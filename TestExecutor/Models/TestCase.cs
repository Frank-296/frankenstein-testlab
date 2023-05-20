using TestLab.Utilities;

namespace TestExecutor.Models;

public class TestCase
{
    public TestCase() => Executions = new HashSet<Execution>();

    public Guid InputFieldId { get; set; } = Guid.NewGuid();

    public String TestCaseId { get; set; }

    public String BusinessProcessId { get; set; }

    public TestType TestType { get; set; }

    public String Name { get; set; }

    public String Description { get; set; }

    public String Preconditions { get; set; }

    public Boolean TestDataIsRequired { get; set; }

    public Byte[] TestData { get; set; }

    public Boolean Status { get; set; }

    public String Remarks { get; set; }

    public virtual BusinessProcess BusinessProcess { get; set; }

    public virtual ICollection<Execution> Executions { get; set; }
}