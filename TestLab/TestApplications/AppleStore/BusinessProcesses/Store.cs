#nullable disable
using TestLab.TestApplications.AppleStore.Locators;

namespace TestLab.TestApplications.AppleStore.BusinessProcesses;

public class Store
{
	static Screenshot screenshot;

	public static void SearchProductStep(IWebDriver driver, List<DataPool> dataPool, ExtentReports report, ExtentTest test)
	{
		TestSteps.SetStepNumber();

		try
		{
			var continueButton = new WebDriverWait(driver, TimeSpan.FromSeconds(5))
				.Until(d => d.FindElement(By.Id(HomeLocators.Locators["continueButton_Id"])));

			if (continueButton.Displayed)
			{
				continueButton.Click();

				var loadPage = new WebDriverWait(driver, TimeSpan.FromSeconds(20))
					.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

				if (loadPage)
				{
					var productOption = driver.FindElement(By.XPath(String.Format(HomeLocators.Locators["searchResult_Xpath"], dataPool
					.FirstOrDefault(x => x.Parameter == "Product").Value)));

					if (productOption.Displayed)
					{
						Assert.That(productOption.Displayed, Is.EqualTo(true));

						productOption.Click();

						Thread.Sleep(2000);

						screenshot = driver.TakeScreenshot();

						var status = Status.Pass;
						var step = TestSteps.Step + " The product search was successful.";
						var evidence = MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot.AsBase64EncodedString, step).Build();

						test.Log(status, step, evidence);
					}
				}
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

			var loadPage = new WebDriverWait(driver, TimeSpan.FromSeconds(20))
				.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

			if (loadPage)
			{
				var searchResult = new WebDriverWait(driver, TimeSpan.FromSeconds(5))
				.Until(d => d.FindElements(By.XPath(String.Format(HomeLocators.Locators["optionSelect_Xpath"], dataPool
					.FirstOrDefault(x => x.Parameter == "Option").Value))));

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