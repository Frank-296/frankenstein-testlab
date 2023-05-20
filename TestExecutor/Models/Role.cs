namespace TestExecutor.Models;

public class Role
{
    public String Id { get; set; }

    public String Name { get; set; }

    public String Description { get; set; }

    public String NormalizedName { get; set; }

    public String ConcurrencyStamp { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; }
}

public class UserRole
{
    public virtual User User { get; set; }

    public virtual Role Role { get; set; }
}