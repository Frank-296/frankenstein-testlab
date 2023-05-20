#nullable disable
using TestLab.TestApplications.MicrosoftStore.BusinessProcesses;
using TestLab.TestApplications.MicrosoftStore.Methods;

namespace TestLab.TestApplications.MicrosoftStore.TestCases;

[TestFixture]
public class GetProductInformationTestCases
{
	public void Search_Product(Byte[] testData, IWebDriver driver, TestEnvironment testEnvironment, String folderpath)
    {
        var description = "Search product in store.";

		var report = Evidences.GenerateIndividualReport(folderpath);
		var test = report.CreateTest(nameof(this.Search_Product), description);
		var dataPool = Mapper.MapData(testData);

		TestSteps.Step = 0;

		Init.LaunchMicrosoftStoreStep(driver, report, test, testEnvironment);
		Store.SearchProductStep(driver, dataPool, report, test);
		Store.SelectProductStep(driver, dataPool, report, test);

		report.Flush();
		driver.Quit();
    }

	// In this method call any test case will you need to execute locally (Right click on "LocalExecution" and click run test).
	[Test]
    public void LocalExecution()
    {

    }
}