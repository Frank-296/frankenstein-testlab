#nullable disable
namespace TestLab.TestApplications.AppleStore.Methods;

public class Init
{
	static Screenshot screenshot;

	public static void LaunchAppleStoreStep(IWebDriver driver, ExtentReports report, ExtentTest test, TestEnvironment testEnvironment)
	{
		TestSteps.SetStepNumber();

		try
		{
			String environmentUrl = testEnvironment switch
			{
				TestEnvironment.Sit => "https://www.apple.com/",
				TestEnvironment.Uat => "https://www.apple.com/",
				TestEnvironment.Dev => "https://www.apple.com/",
				TestEnvironment.Prod => "https://www.apple.com/",
				_ => "https://www.apple.com/",
			};

			driver.Manage().Window.Maximize();
			driver.Navigate().GoToUrl(environmentUrl);

			var loadPage = new WebDriverWait(driver, TimeSpan.FromSeconds(20))
				.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

			if (loadPage)
			{
				Thread.Sleep(2000);

				screenshot = driver.TakeScreenshot();

				var status = Status.Pass;
				var step = TestSteps.Step + " The application launched successfully.";
				var evidence = MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot.AsBase64EncodedString, step).Build();

				test.Log(status, step, evidence);

				Assert.That(loadPage, Is.EqualTo(true));
			}
		}
		catch (Exception exception)
		{
			Thread.Sleep(2000);

			screenshot = driver.TakeScreenshot();

			var status = Status.Fail;
			var step = TestSteps.Step + " The application launch failed.\n" + exception.Message;
			var evidence = MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot.AsBase64EncodedString, step).Build();

			test.Log(status, step, evidence);

			report.Flush();
			driver.Quit();

			TestSteps.Step = 0;
			Assert.Fail(exception.Message);
		}
	}
}