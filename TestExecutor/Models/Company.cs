namespace TestExecutor.Models;

public class Company
{
    public Company() => Users = new HashSet<User>();

    public String CompanyId { get; set; }

    public String Name { get; set; }

    public String Domain { get; set; }

    public virtual ICollection<User> Users { get; set; }
}