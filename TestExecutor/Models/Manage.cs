namespace TestExecutor.Models;

public class LoginViewModel
{
    public String Email { get; set; }

    public String Password { get; set; }

    public Boolean RememberMe { get; set; }
}

public class GetUserTokenViewModel
{
    public String Id { get; set; }

    public String CompanyId { get; set; }

    public String Role { get; set; }

    public String Token { get; set; }

    public String Expiration { get; set; }
}

public class GetProfileViewModel
{
    public String Id { get; set; }

    public String Name { get; set; }

    public String LastName { get; set; }

    public String MothersLastName { get; set; }

    public String DisplayName { get; set; }

    public String Role { get; set; }

    public String Company { get; set; }

    public String Email { get; set; }

    public String PhoneNumber { get; set; }

    public Byte[] ProfilePicture { get; set; }

    public DateTime RegistrationDate { get; set; }

    public DateTime SetPasswordDate { get; set; }

    public Boolean IsAuthorized { get; set; }
}

public class UpdateProfileViewModel
{
    public String Id { get; set; }

    public String Name { get; set; }

    public String LastName { get; set; }

    public String MothersLastName { get; set; }

    public String PhoneNumber { get; set; }

    public Byte[] ProfilePicture { get; set; }
}

public class ChangePasswordViewModel
{
    public String Id { get; set; }

    public String OldPassword { get; set; }

    public String NewPassword { get; set; }

    public String ConfirmPassword { get; set; }
}