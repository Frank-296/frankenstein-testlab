#nullable disable
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

using Api.Data;

namespace Api.Models;

[Table("TestCases")]
public class TestCase
{
    public TestCase() => Executions = new HashSet<Execution>();

    [Key]
    [Required]
    [Display(Name = "Test case")]
    [Column("TestCaseId", TypeName = "uniqueidentifier")]
    public Guid TestCaseId { get; set; }

    [Required]
    public Guid BusinessProcessId { get; set; }

    [Required]
	[Display(Name = "Test type")]
	public TestType TestType { get; set; }

    [Required]
    [Display(Name = "Test case name")]
    public String Name { get; set; }

    [Required]
    [Display(Name = "Test case description")]
    public String Description { get; set; }

    public String Preconditions { get; set; }

    [Required]
    public Boolean TestDataIsRequired { get; set; }

    public Byte[] TestData { get; set; }

    [Required]
    public Boolean Status { get; set; }

    public String Remarks { get; set; }

    public virtual BusinessProcess BusinessProcess { get; set; }

    public virtual ICollection<Execution> Executions { get; set; }
}

public class CreateTestCasesBase
{
    public static async Task Initialize(ApplicationDbContext context, IWebHostEnvironment environment)
	{
		context.Database.EnsureCreated();

        var businessProcesses = context.BusinessProcesses.ToList();

        foreach (var businessProcess in businessProcesses)
        {
            if (businessProcess.TestApplication.Name == "Microsoft Store")
            {
                if (businessProcess.Name == "Get Development Information")
				{
					List<TestCase> testCases = new()
				    {
					    new TestCase()
                        {
                            TestCaseId = Guid.NewGuid(),
                            BusinessProcessId = businessProcess.BusinessProcessId,
                            TestType = TestType.Functional,
                            Name = "Get_Java_Documentation",
                            Description = "Test case to obtain relevant information about java development in Visual Studio.",
                            Preconditions = "N/A",
							TestDataIsRequired = false,
                            TestData = null,
							Status = true,
							Remarks = "This case will show the flow of a failed execution."
						},

						new TestCase()
                        {
                            TestCaseId = Guid.NewGuid(),
                            BusinessProcessId = businessProcess.BusinessProcessId,
                            TestType = TestType.Functional,
                            Name = "Get_Maui_Documentation",
                            Description = "Test case to obtain relevant information about development with .NET MAUI.",
                            Preconditions = "N/A",
                            TestDataIsRequired = true,
                            TestData = GetTestData(businessProcess.TestApplication.Name, businessProcess.Name, "Get_Maui_Documentation", environment),
							Status = true,
							Remarks = "To execute this test case, test data is needed, you can download the one that comes as an example."
						},

						new TestCase()
						{
							TestCaseId = Guid.NewGuid(),
							BusinessProcessId = businessProcess.BusinessProcessId,
							TestType = TestType.Functional,
							Name = "Get_Net_Code_Sample",
							Description = "Test case to get .NET code sample of the selected topic of interest.",
							Preconditions = "N/A",
							TestDataIsRequired = true,
							TestData = GetTestData(businessProcess.TestApplication.Name, businessProcess.Name, "Get_Net_Code_Sample", environment),
							Status = true,
							Remarks = "To execute this test case, test data is needed, you can download the one that comes as an example."
						},

						new TestCase()
						{
							TestCaseId = Guid.NewGuid(),
							BusinessProcessId = businessProcess.BusinessProcessId,
							TestType = TestType.Functional,
							Name = "Get_Net_Migration_Documentation",
							Description = "Test case to obtain relevant information about migrating from ASP.NET Core 6.0 to 7.0.",
							Preconditions = "N/A",
							TestDataIsRequired = false,
							TestData = null,
							Status = true
						}
					};

                    foreach (var testCase in testCases)
					{
						if (!context.TestCases.Any(c => c.Name == testCase.Name && c.BusinessProcessId == testCase.BusinessProcessId))
						{
							context.Add(testCase);
							await context.SaveChangesAsync();
						}
					}
				}

				if (businessProcess.Name == "Get Product Information")
				{
					var testCase = new TestCase()
					{
						TestCaseId = Guid.NewGuid(),
						BusinessProcessId = businessProcess.BusinessProcessId,
						TestType = TestType.Functional,
						Name = "Search_Product",
						Description = "Search product in Microsoft store.",
						Preconditions = "N/A",
						TestDataIsRequired = true,
						TestData = GetTestData(businessProcess.TestApplication.Name, businessProcess.Name, "Search_Product", environment),
						Status = true,
						Remarks = "To execute this test case, test data is needed, you can download the one that comes as an example."
					};

					if (!context.TestCases.Any(c => c.Name == testCase.Name && c.BusinessProcessId == testCase.BusinessProcessId))
					{
						context.Add(testCase);
						await context.SaveChangesAsync();
					}
				}
			}
			if (businessProcess.TestApplication.Name == "Apple Store")
			{
				var testCase = new TestCase()
				{
					TestCaseId = Guid.NewGuid(),
					BusinessProcessId = businessProcess.BusinessProcessId,
					TestType = TestType.Functional,
					Name = "Search_Product",
					Description = "Search product in Apple store.",
					Preconditions = "N/A",
					TestDataIsRequired = true,
					TestData = GetTestData(businessProcess.TestApplication.Name, businessProcess.Name, "Search_Product", environment),
					Status = true,
					Remarks = "To execute this test case, test data is needed, you can download the one that comes as an example."
				};

				if (!context.TestCases.Any(c => c.Name == testCase.Name && c.BusinessProcessId == testCase.BusinessProcessId))
				{
					context.Add(testCase);
					await context.SaveChangesAsync();
				}
			}
		}
	}

    public static Byte[] GetTestData(String testApplication, String businessProcess, String testCaseName, IWebHostEnvironment environment)
    {
        if (testApplication == "Microsoft Store")
        {
            if (businessProcess == "Get Development Information")
			{
				if (testCaseName == "Get_Maui_Documentation")
				{
					var file = Path.Combine(environment.WebRootPath, "testData", "microsoftStore", "getDevelopmentInformationTestCases", $"{testCaseName}.xlsx");

					return File.ReadAllBytes(file);
				}
                else
				{
					var file = Path.Combine(environment.WebRootPath, "testData", "microsoftStore", "getDevelopmentInformationTestCases", $"{testCaseName}.xlsx");

					return File.ReadAllBytes(file);
				}
			}
            else
			{
				var file = Path.Combine(environment.WebRootPath, "testData", "microsoftStore", "getProductInformationTestCases", $"{testCaseName}.xlsx");

				return File.ReadAllBytes(file);
			}
        }
        else
		{
			var file = Path.Combine(environment.WebRootPath, "testData", "appleStore", "getProductInformationTestCases", $"{testCaseName}.xlsx");

			return File.ReadAllBytes(file);
		}
    }
}

public enum TestType : Int32
{
    Smoke = 1,
    Functional = 2,
    Regression = 3,
    Acceptance = 4,
}