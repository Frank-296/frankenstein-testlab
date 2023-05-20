#nullable disable
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

using Api.Data;

namespace Api.Models;

[Table("BusinessProcesses")]
public class BusinessProcess
{
    public BusinessProcess() => TestCases = new HashSet<TestCase>();

    [Key]
    [Required]
    [Display(Name = "Business process")]
    [Column("BusinessProcessId", TypeName = "uniqueidentifier")]
    public Guid BusinessProcessId { get; set; }

    [Required]
    public Guid TestApplicationId { get; set; }

    [Required]
    [Display(Name = "Business process name")]
    public String Name { get; set; }

    [Required]
    public String NameSpace { get; set; }

    [Required]
    [Display(Name = "Business process description")]
    public String Description { get; set; }

    [Required]
    public Boolean Status { get; set; }

    public String Remarks { get; set; }

    public virtual TestApplication TestApplication { get; set; }

    public virtual ICollection<TestCase> TestCases { get; set; }
}

public class CreateBusinessProcessesBase
{
    public static async Task Initialize(ApplicationDbContext context)
	{
		context.Database.EnsureCreated();

        var testApplications = context.TestApplications.ToList();

		foreach (var testApplication in testApplications)
		{
			if (testApplication.Name == "Microsoft Store")
			{
                List<BusinessProcess> businessProcesses = new()
                {
                    new BusinessProcess()
                    {
                        BusinessProcessId = Guid.NewGuid(),
                        TestApplicationId = testApplication.TestApplicationId,
                        Name = "Get Development Information",
                        NameSpace = "TestLab.TestApplications.MicrosoftStore.TestCases.GetDevelopmentInformationTestCases",
                        Description = "Business process for obtaining relevant information about developing with .NET.",
                        Status = true
					},

                    new BusinessProcess()
					{
						BusinessProcessId = Guid.NewGuid(),
						TestApplicationId = testApplication.TestApplicationId,
						Name = "Get Product Information",
						NameSpace = "TestLab.TestApplications.MicrosoftStore.TestCases.GetProductInformationTestCases",
						Description = "Business process for obtaining relevant information about Microsoft products.",
						Status = true
					}
                };

                foreach (var businessProcess in businessProcesses)
				{
					if (!context.BusinessProcesses.Any(p => p.Name == businessProcess.Name && businessProcess.TestApplicationId == businessProcess.TestApplicationId))
					{
                        context.Add(businessProcess);
                        await context.SaveChangesAsync();
                    }
                }
			}

            if (testApplication.Name == "Apple Store")
			{
                var businessProcess = new BusinessProcess()
                {
                    BusinessProcessId = Guid.NewGuid(),
                    TestApplicationId = testApplication.TestApplicationId,
                    Name = "Get Product Information",
                    NameSpace = "TestLab.TestApplications.AppleStore.TestCases.GetProductInformationTestCases",
                    Description = "Business process for obtaining relevant information about Apple products.",
                    Status = true
                };

				if (!context.BusinessProcesses.Any(p => p.Name == businessProcess.Name && businessProcess.TestApplicationId == businessProcess.TestApplicationId))
				{
					context.Add(businessProcess);
					await context.SaveChangesAsync();
				}
			}
		}
	}
}