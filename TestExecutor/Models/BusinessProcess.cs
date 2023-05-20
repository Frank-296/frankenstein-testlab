namespace TestExecutor.Models;

public class BusinessProcess
{
    public BusinessProcess() => TestCases = new HashSet<TestCase>();

    public String BusinessProcessId { get; set; }

    public String TestApplicationId { get; set; }

    public String Name { get; set; }

    public String NameSpace { get; set; }

    public String Description { get; set; }

    public Boolean Status { get; set; }

    public String Remarks { get; set; }

    public virtual TestApplication TestApplication { get; set; }

    public virtual ICollection<TestCase> TestCases { get; set; }
}