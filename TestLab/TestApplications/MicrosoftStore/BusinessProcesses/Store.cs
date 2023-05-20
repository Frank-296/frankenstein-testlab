#nullable disable
using TestLab.TestApplications.MicrosoftStore.Locators;

namespace TestLab.TestApplications.MicrosoftStore.BusinessProcesses;

public class Store
{
	static Screenshot screenshot;

	public static void SearchProductStep(IWebDriver driver, List<DataPool> dataPool, ExtentReports report, ExtentTest test)
	{
		TestSteps.SetStepNumber();

		try
		{
			var searchButton = driver.FindElement(By.Id(HomeLocators.Locators["searchButton_Id"]));

			searchButton.Click();

			var searchInput = new WebDriverWait(driver, TimeSpan.FromSeconds(5))
				.Until(d => d.FindElement(By.Id(HomeLocators.Locators["searchInput_Id"])));

			if (searchInput.Displayed)
			{
				Assert.That(searchInput.Displayed, Is.EqualTo(true));

				driver.FindElement(By.Id(HomeLocators.Locators["searchInput_Id"]))
					.SendKeys(dataPool.FirstOrDefault(x => x.Parameter == "SearchValue").Value);

				Thread.Sleep(2000);

				screenshot = driver.TakeScreenshot();

				var status = Status.Pass;
				var step = TestSteps.Step + " The product search was successful.";
				var evidence = MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot.AsBase64EncodedString, step).Build();

				test.Log(status, step, evidence);
			}
		}
		catch (Exception exception)
		{
			Thread.Sleep(2000);

			screenshot = driver.TakeScreenshot();

			var status = Status.Fail;
			var step = TestSteps.Step + " The product search failed.\n" + exception.Message;
			var evidence = MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot.AsBase64EncodedString, step).Build();

			test.Log(status, step, evidence);

			report.Flush();
			driver.Quit();

			TestSteps.Step = 0;
			Assert.Fail(exception.Message);
		}
	}

	public static void SelectProductStep(IWebDriver driver, List<DataPool> dataPool, ExtentReports report, ExtentTest test)
	{
		TestSteps.SetStepNumber();

		try
		{
			var searchResult = new WebDriverWait(driver, TimeSpan.FromSeconds(5))
				.Until(d => d.FindElements(By.XPath(String.Format(HomeLocators.Locators["searchResult_Xpath"], dataPool
					.FirstOrDefault(x => x.Parameter == "SearchValue").Value))));

			if (searchResult.Count > 0)
			{
				Assert.That(searchResult.FirstOrDefault().Displayed, Is.EqualTo(true));

				searchResult.FirstOrDefault().Click();

				Thread.Sleep(2000);

				screenshot = driver.TakeScreenshot();

				var status = Status.Pass;
				var step = TestSteps.Step + " The product selection was successfully displayed.";
				var evidence = MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot.AsBase64EncodedString, step).Build();

				test.Log(status, step, evidence);
			}
		}
		catch (Exception exception)
		{
			Thread.Sleep(2000);

			screenshot = driver.TakeScreenshot();

			var status = Status.Fail;
			var step = TestSteps.Step + " The product selection failed.\n" + exception.Message;
			var evidence = MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot.AsBase64EncodedString, step).Build();

			test.Log(status, step, evidence);

			report.Flush();
			driver.Quit();

			TestSteps.Step = 0;
			Assert.Fail(exception.Message);
		}
	}
}