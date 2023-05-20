#nullable disable
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

using Api.Data;

namespace Api.Models;

[Table("TestApplications")]
public class TestApplication
{
    public TestApplication() => BusinessProcesses = new HashSet<BusinessProcess>();

    [Key]
    [Required]
    [Display(Name = "Test application")]
    [Column("TestApplicationId", TypeName = "uniqueidentifier")]
    public Guid TestApplicationId { get; set; }

    [Required]
    [Display(Name = "Test application name")]
    public String Name { get; set; }

    [Required]
    public String NameSpace { get; set; }

    [Required]
    [Display(Name = "Test application description")]
    public String Description { get; set; }

    [Required]
    public Boolean Status { get; set; }

    public String Remarks { get; set; }

    public virtual ICollection<BusinessProcess> BusinessProcesses { get; set; }
}

public class CreateTestApplicationsBase
{
    public static async Task Initialize(ApplicationDbContext context)
    {
        context.Database.EnsureCreated();

        List<TestApplication> testApplications = new()
        {
            new TestApplication()
            {
                TestApplicationId = Guid.NewGuid(),
                Name = "Apple Store",
                NameSpace = "TestLab.TestApplications.AppleStore.TestCases",
                Description = "Application to obtain information about the products offered by Apple.",
                Status = true
            },

            new TestApplication()
			{
				TestApplicationId = Guid.NewGuid(),
				Name = "Microsoft Store",
				NameSpace = "TestLab.TestApplications.MicrosoftStore.TestCases",
				Description = "Application to obtain information about the products offered by Microsoft.",
				Status = true
			}
        };

        foreach (var testApplication in testApplications)
        {
            if (!context.TestApplications.Any(a => a.Name == testApplication.Name))
            {
                context.Add(testApplication);
                await context.SaveChangesAsync();
            }
        }
    }
}