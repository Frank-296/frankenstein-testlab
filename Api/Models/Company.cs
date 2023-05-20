#nullable disable
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

using Api.Data;

namespace Api.Models;

public class Company
{
    public Company() => Users = new HashSet<User>();

    [Key]
    [Required]
    [Column("CompanyId", TypeName = "uniqueidentifier")]
    public Guid CompanyId { get; set; }

    [Required]
    [Display(Name = "Company")]
    public String Name { get; set; }

    [Required]
    public String Domain { get; set; }

    public virtual ICollection<User> Users { get; set; }
}

public class CreateCompanyBase
{
    public static async Task Initialize(ApplicationDbContext context)
    {
        context.Database.EnsureCreated();

        var company = new Company
        {
            Name = "your_company",
            Domain = "@your_domain"
        };

        if (!context.Companies.Any(c => c.Name == company.Name))
        {
            context.Add(company);
            await context.SaveChangesAsync();
        }
    }
}