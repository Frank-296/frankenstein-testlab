#nullable disable
using TestLab.TestApplications.MicrosoftStore.Locators;

namespace TestLab.TestApplications.MicrosoftStore.BusinessProcesses;

public class DeveloperCenter
{
    static Screenshot screenshot;

    public static void ExploreDocumentationStep(IWebDriver driver, List<DataPool> dataPool, ExtentReports report, ExtentTest test)
    {
        TestSteps.SetStepNumber();

        try
        {
            var loadPage = new WebDriverWait(driver, TimeSpan.FromSeconds(20))
                .Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

            if (loadPage)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", driver
                    .FindElement(By.XPath(String.Format(DeveloperLocators.Locators["navOption_Xpath"], dataPool
                        .FirstOrDefault(x => x.Parameter == "MenuOption").Value))));

                var menuOption = driver.FindElement(By.XPath(String.Format(DeveloperLocators.Locators["navOption_Xpath"], dataPool
                    .FirstOrDefault(x => x.Parameter == "MenuOption").Value)));

                if (menuOption.Displayed)
                {
                    Assert.That(menuOption.Displayed, Is.EqualTo(true));

                    menuOption.Click();

					Thread.Sleep(2000);

					screenshot = driver.TakeScreenshot();

					var status = Status.Pass;
                    var step = TestSteps.Step + " The menu option selection was successful.";
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
            var step = TestSteps.Step + " The menu option selection failed.\n" + exception.Message;
            var evidence = MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot.AsBase64EncodedString, step).Build();

            test.Log(status, step, evidence);

            report.Flush();
            driver.Quit();

            TestSteps.Step = 0;
            Assert.Fail(exception.Message);
        }
    }

    public static void SearchDocumentationStep(IWebDriver driver, List<DataPool> dataPool, ExtentReports report, ExtentTest test)
    {
        TestSteps.SetStepNumber();

        try
        {
            var loadPage = new WebDriverWait(driver, TimeSpan.FromSeconds(20))
                .Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

            if (loadPage)
            {
                driver.FindElement(By.Id(LearnLocators.Locators["searchInput_Id"]))
                    .SendKeys(dataPool.FirstOrDefault(x => x.Parameter == "SearchValue").Value);

                driver.FindElement(By.Id(LearnLocators.Locators["searchButton_Id"])).Click();

				Thread.Sleep(2000);

				screenshot = driver.TakeScreenshot();

				var status = Status.Pass;
                var step = TestSteps.Step + " The documentation search was successful.";
				var evidence = MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot.AsBase64EncodedString, step).Build();

				test.Log(status, step, evidence);

                var contentArea = new WebDriverWait(driver, TimeSpan.FromSeconds(5))
                    .Until(d => d.FindElement(By.XPath(LearnLocators.Locators["contentArea_Xpath"])));

                Assert.That(contentArea.Displayed, Is.EqualTo(true));
            }
        }
        catch (Exception exception)
        {
            Thread.Sleep(2000);

            screenshot = driver.TakeScreenshot();

            var status = Status.Fail;
            var step = TestSteps.Step + " The documentation search failed.\n" + exception.Message;
            var evidence = MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot.AsBase64EncodedString, step).Build();

            test.Log(status, step, evidence);

            report.Flush();
            driver.Quit();

            TestSteps.Step = 0;
            Assert.Fail(exception.Message);
        }
    }

    public static void ShowSearchDocumentationStep(IWebDriver driver, List<DataPool> dataPool, ExtentReports report, ExtentTest test)
    {
        TestSteps.SetStepNumber();

        try
        {
            var loadPage = new WebDriverWait(driver, TimeSpan.FromSeconds(20))
                .Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

            if (loadPage)
            {
                driver.FindElement(By.XPath(String.Format(LearnLocators.Locators["contentAreaRadio_Xpath"], dataPool
                    .FirstOrDefault(x => x.Parameter == "ContentArea").Value))).Click();

                var searchResult = new WebDriverWait(driver, TimeSpan.FromSeconds(5))
                    .Until(d => d.FindElement(By.XPath(String.Format(LearnLocators.Locators["searchResult_Xpath"], dataPool
                        .FirstOrDefault(x => x.Parameter == "SearchValue").Value))));

                if (searchResult.Displayed)
                {
                    Assert.That(searchResult.Displayed, Is.EqualTo(true));

                    searchResult.Click();

					Thread.Sleep(2000);

					screenshot = driver.TakeScreenshot();

					var status = Status.Pass;
                    var step = TestSteps.Step + " The searched documentation was successfully displayed.";
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
            var step = TestSteps.Step + " The requested documentation was not found.\n" + exception.Message;
            var evidence = MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot.AsBase64EncodedString, step).Build();

            test.Log(status, step, evidence);

            report.Flush();
            driver.Quit();

            TestSteps.Step = 0;
            Assert.Fail(exception.Message);
        }
    }
}