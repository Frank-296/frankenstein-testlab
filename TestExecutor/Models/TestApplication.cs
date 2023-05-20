namespace TestExecutor.Models;

public class TestApplication
{
    public TestApplication() => BusinessProcesses = new HashSet<BusinessProcess>();

    public String TestApplicationId { get; set; }

    public String Name { get; set; }

    public String NameSpace { get; set; }

    public String Description { get; set; }

    public Boolean Status { get; set; }

    public String Remarks { get; set; }

    public virtual ICollection<BusinessProcess> BusinessProcesses { get; set; }
}