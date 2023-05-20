#nullable disable
using TestLab.TestApplications.MicrosoftStore.Locators;

namespace TestLab.TestApplications.MicrosoftStore.Sections;

public class FooterSection
{
    static Screenshot screenshot;

    public static void SelectFooterSectionStep(IWebDriver driver, List<DataPool> dataPool, ExtentReports report, ExtentTest test)
    {
        TestSteps.SetStepNumber();

        try
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", driver
                .FindElement(By.XPath(String.Format(HomeLocators.Locators["footerSection_Xpath"], dataPool
                    .FirstOrDefault(x => x.Parameter == "FooterSection").Value))));

            var footerSection = driver.FindElement(By.XPath(String.Format(HomeLocators.Locators["footerSection_Xpath"], dataPool
                    .FirstOrDefault(x => x.Parameter == "FooterSection").Value)));

            if (footerSection.Displayed)
            {
                Assert.That(footerSection.Displayed, Is.EqualTo(true));

                footerSection.Click();

				Thread.Sleep(2000);

				screenshot = driver.TakeScreenshot();

				var status = Status.Pass;
                var step = TestSteps.Step + " The footer section selection was successful.";
				var evidence = MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot.AsBase64EncodedString, step).Build();

				test.Log(status, step, evidence);
            }
        }
        catch (Exception exception)
        {
            Thread.Sleep(2000);

            screenshot = driver.TakeScreenshot();

            var status = Status.Fail;
            var step = TestSteps.Step + " The footer section selection failed.\n" + exception.Message;
            var evidence = MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot.AsBase64EncodedString, step).Build();

            test.Log(status, step, evidence);

            report.Flush();
            driver.Quit();

            TestSteps.Step = 0;
            Assert.Fail(exception.Message);
        }
    }
}