namespace TestExecutor.Models;

public class User
{
    public String Id { get; set; }

    public String CompanyId { get; set; }

    public String DisplayName { get; set; }

    public String Email { get; set; }

    public String PhoneNumber { get; set; }

    public Byte[] ProfilePicture { get; set; }

    public DateTime RegistrationDate { get; set; }

    public DateTime SetPasswordDate { get; set; }

    public Boolean IsAuthorized { get; set; }

    public virtual Company Company { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; }
}

public class RegisterUserViewModel
{
    public String Email { get; set; }

    public String Role { get; set; }

    public String CompanyId { get; set; }

    public DateTime RegistrationDate { get; set; }

    public DateTime SetPasswordDate { get; set; }

    public Boolean IsAuthorized { get; set; }
}

public class UpdateUserViewModel
{
    public String Id { get; set; }

    public String Role { get; set; }

    public Boolean IsAuthorized { get; set; }
}