#nullable disable
namespace TestLab.Utilities;

public class Test
{
	public ExecutionStatus ExecutionStatus { get; set; }

	public String ReportPath { get; set; }
}

public class Evidences
{
	public static Test Run(String testApplicationNameSpace, String businessProcessNameSpace, String testCaseName, Byte[] testData, BrowserDriver browser, TestEnvironment testEnvironment, String folderpath)
	{
		try
		{
			var testClassType = Type.GetType(businessProcessNameSpace, true);
			var testClassInstance = Activator.CreateInstance(testClassType);

			var testCasesClasses = Assembly.GetExecutingAssembly().GetTypes()
				.Where(t => t.Namespace == testApplicationNameSpace);

			foreach (var testCaseClass in testCasesClasses)
			{
				var methods = testCaseClass.GetMethods().ToList();

				if (methods.Any(m => m.Name == testCaseName))
				{
					var driver = SetDriver.SelectBrowser(browser);

					if (driver != null)
					{
						try
						{
							var method = methods.FirstOrDefault(m => m.Name == testCaseName);

							if (testData != null)
								method.Invoke(testClassInstance, new Object[] { testData, driver, testEnvironment, folderpath });
							else
								method.Invoke(testClassInstance, new Object[] { driver, testEnvironment, folderpath });

							return new Test { ExecutionStatus = ExecutionStatus.Pass, ReportPath = folderpath };
						}
						catch (Exception)
						{
							return new Test { ExecutionStatus = ExecutionStatus.Fail, ReportPath = folderpath };
						}
					}

					return null;
				}
			}

			return new Test { ExecutionStatus = ExecutionStatus.DontExist, ReportPath = null };
		}
		catch (Exception)
		{
			return new Test { ExecutionStatus = ExecutionStatus.Fail, ReportPath = folderpath };
		}
	}

	public static ExtentReports GenerateIndividualReport(String folderpath)
	{
		var htmlReporter = new ExtentHtmlReporter(folderpath + @"\");
		var extentReport = new ExtentReports();

		extentReport.AttachReporter(htmlReporter);
		extentReport.AddSystemInfo("OS", RuntimeInformation.OSDescription);
		extentReport.AddSystemInfo("Host name", Dns.GetHostName());
		extentReport.AddSystemInfo("Department", "QA");
		extentReport.AddSystemInfo("Username", Environment.UserName);

		htmlReporter.Config.Theme = Theme.Standard;
		htmlReporter.Config.Encoding = "UTF-8";
		htmlReporter.Config.DocumentTitle = "Execution report";
		htmlReporter.Config.ReportName = "Automation report";

		return extentReport;
	}

	public static String GetLocalPath(String applicationName, String className, String methodName)
	{
		var localPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
		var resultsPath = Path.Combine(localPath, "TestLab", "TestApplications", applicationName, "Results", className);

		if (!Directory.Exists(resultsPath))
			Directory.CreateDirectory(resultsPath);

		var folderpath = Path.Combine(localPath, "TestLab", "TestApplications", applicationName, "Results", className, methodName);

		if (!Directory.Exists(folderpath))
			Directory.CreateDirectory(folderpath);

		return folderpath;
	}
}

public enum TestType : Int32
{
    [Display(Name = "Smoke test")]
    Smoke = 1,
    [Display(Name = "Functional test")]
    Functional = 2,
    [Display(Name = "Regression test")]
    Regression = 3,
    [Display(Name = "Acceptance test")]
    Acceptance = 4,
}

public enum TestEnvironment : Int32
{
    [Display(Name = "SIT")]
    Sit = 1,
    [Display(Name = "UAT")]
    Uat = 2,
    [Display(Name = "DEVELOPMENT")]
    Dev = 3,
    [Display(Name = "PRODUCTION")]
    Prod = 4
}

public enum ExecutionStatus : Int32
{
    [Display(Name = "Dont exist")]
    DontExist = 0,
    [Display(Name = "Not run")]
    NotRun = 1,
    [Display(Name = "Stop")]
    Stop = 2,
    [Display(Name = "Fail")]
    Fail = 3,
    [Display(Name = "Pass")]
    Pass = 4
}