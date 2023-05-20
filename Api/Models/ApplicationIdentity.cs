#nullable disable
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

using Api.Data;

namespace Api.Models;

public class ApplicationIdentity { }

public class User : IdentityUser<Guid>
{
    public User() : base() { }

    public User(String name) : base(name) { }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public override Guid Id
    {
        get => base.Id;
        set => base.Id = value;
    }

    public Guid? CompanyId { get; set; }

    [StringLength(256)]
    public String Name { get; set; }

    [StringLength(256)]
    public String LastName { get; set; }

    [StringLength(256)]
    public String MothersLastName { get; set; }

    [StringLength(16)]
    [Display(Name = "Phone number")]
    public override String PhoneNumber { get; set; }

    public Byte[] ProfilePicture { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime RegistrationDate { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime SetPasswordDate { get; set; }

    public Boolean IsAuthorized { get; set; }

    [NotMapped]
    [Display(Name = "Executor")]
    public String DisplayName => String.Format("{0} {1} {2}", Name, LastName, MothersLastName);

    public virtual Company Company { get; set; }

    public virtual ICollection<UserClaim> Claims { get; set; }

    public virtual ICollection<UserLogin> Logins { get; set; }

    public virtual ICollection<UserToken> Tokens { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; }
}

public class Role : IdentityRole<Guid>
{
    public Role() : base() { }

    public Role(String name) : base(name) { }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public override Guid Id
    {
        get => base.Id;
        set => base.Id = value;
    }

    [Required]
    public override String Name { get; set; }

    public String Description { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; }

    public virtual ICollection<RoleClaim> RoleClaims { get; set; }
}

public class UserRole : IdentityUserRole<Guid>
{
    public virtual User User { get; set; }

    public virtual Role Role { get; set; }
}

public class UserClaim : IdentityUserClaim<Guid>
{
    public virtual User User { get; set; }
}

public class UserLogin : IdentityUserLogin<Guid>
{
    public virtual User User { get; set; }
}

public class UserToken : IdentityUserToken<Guid>
{
    public virtual User User { get; set; }
}

public class RoleClaim : IdentityRoleClaim<Guid>
{
    public virtual Role Role { get; set; }
}

public class ApplicationManager
{
    public static async Task Initialize(ApplicationDbContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
    {
        context.Database.EnsureCreated();

        String email = "your_user_email";
        String password = "your_password";

        List<String> roles = new()
        {
            "Root",
            "Manager",
            "Administrator",
            "Developer",
            "Automator",
            "Tester",
            "User"
        };

        foreach (var role in roles)
            if (await roleManager.FindByNameAsync(role) == null)
                await roleManager.CreateAsync(new Role(role));

        if (await userManager.FindByEmailAsync(email) == null)
        {
            var company = await context.Companies.Where(c => c.Name == "your_company")
                .FirstOrDefaultAsync();

            if (company != null)
            {
                var user = new User
                {
                    CompanyId = company.CompanyId,
                    UserName = email,
                    Email = email,
                    RegistrationDate = DateTime.Now,
                    SetPasswordDate = DateTime.Now,
                    EmailConfirmed = true,
                    IsAuthorized = true
                };

                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                    await userManager.AddToRoleAsync(user, "Root");
            }
        }
    }
}