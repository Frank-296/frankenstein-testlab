#nullable disable
using TestLab.TestApplications.MicrosoftStore.BusinessProcesses;
using TestLab.TestApplications.MicrosoftStore.Sections;
using TestLab.TestApplications.MicrosoftStore.Methods;

namespace TestLab.TestApplications.MicrosoftStore.TestCases;

[TestFixture]
public class GetDevelopmentInformationTestCases
{
    public void Get_Maui_Documentation(Byte[] testData, IWebDriver driver, TestEnvironment testEnvironment, String folderpath)
    {
        var description = "Get information about Supported platforms for .NET MAUI apps.";

        var report = Evidences.GenerateIndividualReport(folderpath);
        var test = report.CreateTest(nameof(this.Get_Maui_Documentation), description);
        var dataPool = Mapper.MapData(testData);

        TestSteps.Step = 0;

        Init.LaunchMicrosoftStoreStep(driver, report, test, testEnvironment);
        FooterSection.SelectFooterSectionStep(driver, dataPool, report, test);
        DeveloperCenter.ExploreDocumentationStep(driver, dataPool, report, test);
        LanguageSection.ChangeLanguageStep(driver, dataPool, report, test);
        DeveloperCenter.SearchDocumentationStep(driver, dataPool, report, test);
        DeveloperCenter.ShowSearchDocumentationStep(driver, dataPool, report, test);

        report.Flush();
        driver.Quit();
    }

    public void Get_Net_Migration_Documentation(IWebDriver driver, TestEnvironment testEnvironment, String folderpath)
    {
        var description = "Test case to obtain relevant information about migrating from ASP.NET Core 6.0 to 7.0.";

        var report = Evidences.GenerateIndividualReport(folderpath);
        var test = report.CreateTest(nameof(this.Get_Net_Migration_Documentation), description);
        var dataPool = new List<DataPool>
        {
            new DataPool() { Parameter = "FooterSection", Value = "Developer Center", Description = "Footer section to select." },
            new DataPool() { Parameter = "Button", Value = "Explore a community", Description = "Button to select." },
            new DataPool() { Parameter = "MenuOption", Value = "Documentation", Description = "Option to select from the Developer Communities menu." },
            new DataPool() { Parameter = "Language", Value = "Español (México)", Description = "Language to select." },
            new DataPool() { Parameter = "SearchValue", Value = "Migración de ASP.NET Core 6.0 a 7.0", Description = "Key word to search for." },
            new DataPool() { Parameter = "ContentArea", Value = "Documentación", Description = "Content area where you want to search." }
        };

        TestSteps.Step = 0;

        Init.LaunchMicrosoftStoreStep(driver, report, test, testEnvironment);
        FooterSection.SelectFooterSectionStep(driver, dataPool, report, test);
        DeveloperCenter.ExploreDocumentationStep(driver, dataPool, report, test);
        LanguageSection.ChangeLanguageStep(driver, dataPool, report, test);
        DeveloperCenter.SearchDocumentationStep(driver, dataPool, report, test);
        DeveloperCenter.ShowSearchDocumentationStep(driver, dataPool, report, test);

        report.Flush();
        driver.Quit();
    }

    public void Get_Java_Documentation(IWebDriver driver, TestEnvironment testEnvironment, String folderpath)
    {
        var description = "Test case to obtain relevant information about java development in Visual Studio.";

        var report = Evidences.GenerateIndividualReport(folderpath);
        var test = report.CreateTest(nameof(this.Get_Java_Documentation), description);
        var dataPool = new List<DataPool>
        {
            new DataPool() { Parameter = "FooterSection", Value = "Developer Center", Description = "Footer section to select." },
            new DataPool() { Parameter = "Button", Value = "Explore a community", Description = "Button to select." },
            new DataPool() { Parameter = "MenuOption", Value = "Documentation", Description = "Option to select from the Developer Communities menu." },
            new DataPool() { Parameter = "Language", Value = "Español (México)", Description = "Language to select." },
            new DataPool() { Parameter = "SearchValue", Value = "Creación de una aplicación Java en Visual Studio", Description = "Key word to search for." },
            new DataPool() { Parameter = "ContentArea", Value = "Documentación", Description = "Content area where you want to search." }
        };

        TestSteps.Step = 0;

        Init.LaunchMicrosoftStoreStep(driver, report, test, testEnvironment);
        FooterSection.SelectFooterSectionStep(driver, dataPool, report, test);
        DeveloperCenter.ExploreDocumentationStep(driver, dataPool, report, test);
        LanguageSection.ChangeLanguageStep(driver, dataPool, report, test);
        DeveloperCenter.SearchDocumentationStep(driver, dataPool, report, test);
        DeveloperCenter.ShowSearchDocumentationStep(driver, dataPool, report, test);

        report.Flush();
        driver.Quit();
    }

    public void Get_Net_Code_Sample(Byte[] testData, IWebDriver driver, TestEnvironment testEnvironment, String folderpath)
    {
        var description = "Get .NET code sample of the selected topic of interest.";

        var report = Evidences.GenerateIndividualReport(folderpath);
        var test = report.CreateTest(nameof(this.Get_Net_Code_Sample), description);
        var dataPool = Mapper.MapData(testData);

        TestSteps.Step = 0;

        Init.LaunchMicrosoftStoreStep(driver, report, test, testEnvironment);
        FooterSection.SelectFooterSectionStep(driver, dataPool, report, test);
        DeveloperCenter.ExploreDocumentationStep(driver, dataPool, report, test);
        GetCodeSample.ExploreCodeSamplesStep(driver, dataPool, report, test);
        GetCodeSample.SearchCodeSampleStep(driver, dataPool, report, test);
        GetCodeSample.ShowCodeSampleStep(driver, dataPool, report, test);

        report.Flush();
        driver.Quit();
    }

	// In this method call any test case will you need to execute locally (Right click on "LocalExecution" and click run test).
	[Test]
    public void LocalExecution()
    {
		// In the third parameter, pass it the name of the test case so that its folder is created where the report will be saved.
		var folderpath = Evidences.GetLocalPath("MicrosoftStore", this.GetType().Name, nameof(this.Get_Java_Documentation));

		Get_Java_Documentation(new ChromeDriver(), TestEnvironment.Sit, folderpath);
    }
}