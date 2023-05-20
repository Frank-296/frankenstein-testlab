#nullable disable
namespace Api.Models;

public class RegisterUserViewModel
{
    public String Email { get; set; }

    public String Role { get; set; }

    public Guid? CompanyId { get; set; }

    public DateTime RegistrationDate { get; set; }

    public DateTime SetPasswordDate { get; set; }

    public Boolean IsAuthorized { get; set; }
}

public class UpdateUserViewModel
{
    public Guid Id { get; set; }

    public String Role { get; set; }

    public Boolean IsAuthorized { get; set; }
}