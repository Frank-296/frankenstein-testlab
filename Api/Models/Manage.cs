#nullable disable
namespace Api.Models;

public class LoginViewModel
{
    public String Email { get; set; }

    public String Password { get; set; }

    public Boolean RememberMe { get; set; }
}

public class UpdateProfileViewModel
{
    public Guid Id { get; set; }

    public String Name { get; set; }

    public String LastName { get; set; }

    public String MothersLastName { get; set; }

    public String PhoneNumber { get; set; }

    public Byte[] ProfilePicture { get; set; }
}

public class ChangePasswordViewModel
{
    public Guid Id { get; set; }

    public String OldPassword { get; set; }

    public String NewPassword { get; set; }
}