#nullable disable
using TestLab.TestApplications.MicrosoftStore.Locators;

namespace TestLab.TestApplications.MicrosoftStore.BusinessProcesses;

public class GetCodeSample
{
    static Screenshot screenshot;

    public static void ExploreCodeSamplesStep(IWebDriver driver, List<DataPool> dataPool, ExtentReports report, ExtentTest test)
    {
        TestSteps.SetStepNumber();

        try
        {
            var loadPage = new WebDriverWait(driver, TimeSpan.FromSeconds(20))
                .Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

            if (loadPage)
            {
                var codeSamplesButton = driver.FindElement(By.XPath(String.Format(DeveloperLocators.Locators["navOption_Xpath"], dataPool
                    .FirstOrDefault(x => x.Parameter == "NavOption").Value)));

                if (codeSamplesButton.Displayed)
                {
                    Assert.That(codeSamplesButton.Displayed, Is.EqualTo(true));

                    codeSamplesButton.Click();

					Thread.Sleep(2000);

					screenshot = driver.TakeScreenshot();

					var status = Status.Pass;
                    var step = TestSteps.Step + " The nav option selection was successful.";
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
            var step = TestSteps.Step + " The nav option selection failed.\n" + exception.Message;
            var evidence = MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot.AsBase64EncodedString, step).Build();

            test.Log(status, step, evidence);

            report.Flush();
            driver.Quit();

            TestSteps.Step = 0;
            Assert.Fail(exception.Message);
        }
    }

    public static void SearchCodeSampleStep(IWebDriver driver, List<DataPool> dataPool, ExtentReports report, ExtentTest test)
    {
        TestSteps.SetStepNumber();

        try
        {
            var loadPage = new WebDriverWait(driver, TimeSpan.FromSeconds(20))
                .Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

            if (loadPage)
            {
                driver.FindElement(By.Id(CodeSamplesLocators.Locators["searchCodeSampleInput_Id"]))
                    .SendKeys(dataPool.FirstOrDefault(x => x.Parameter == "SearchValue").Value);

                driver.FindElement(By.Id(CodeSamplesLocators.Locators["searchCodeSampleButton_Id"])).Click();

				Thread.Sleep(2000);

				screenshot = driver.TakeScreenshot();

				var status = Status.Pass;
                var step = TestSteps.Step + " The code sample search was successful.";
				var evidence = MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot.AsBase64EncodedString, step).Build();

				test.Log(status, step, evidence);

                var productsArea = new WebDriverWait(driver, TimeSpan.FromSeconds(5))
                    .Until(d => d.FindElement(By.XPath(CodeSamplesLocators.Locators["products_Xpath"])));

                Assert.That(productsArea.Displayed, Is.EqualTo(true));
            }
        }
        catch (Exception exception)
        {
            Thread.Sleep(2000);

            screenshot = driver.TakeScreenshot();

            var status = Status.Fail;
            var step = TestSteps.Step + " The code sample search failed.\n" + exception.Message;
            var evidence = MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot.AsBase64EncodedString, step).Build();

            test.Log(status, step, evidence);

            report.Flush();
            driver.Quit();

            TestSteps.Step = 0;
            Assert.Fail(exception.Message);
        }
    }

    public static void ShowCodeSampleStep(IWebDriver driver, List<DataPool> dataPool, ExtentReports report, ExtentTest test)
    {
        TestSteps.SetStepNumber();

        try
        {
            var loadPage = new WebDriverWait(driver, TimeSpan.FromSeconds(20))
                .Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

            if (loadPage)
            {
                driver.FindElement(By.XPath(String.Format(CodeSamplesLocators.Locators["productsAreaCheck_Xpath"], dataPool
                    .FirstOrDefault(x => x.Parameter == "Product").Value))).Click();

                var searchResultCards = new WebDriverWait(driver, TimeSpan.FromSeconds(5))
                    .Until(d => d.FindElements(By.ClassName("card-content-title")));

                Thread.Sleep(2000);

                if (searchResultCards.Count > 0)
                {
                    Assert.That(searchResultCards.FirstOrDefault().Displayed, Is.EqualTo(true));

                    searchResultCards.FirstOrDefault().Click();

					Thread.Sleep(2000);

					screenshot = driver.TakeScreenshot();

					var status = Status.Pass;
                    var step = TestSteps.Step + " The searched code sample was successfully displayed.";
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
            var step = TestSteps.Step + " The requested code sample was not found.\n" + exception.Message;
            var evidence = MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot.AsBase64EncodedString, step).Build();

            test.Log(status, step, evidence);

            report.Flush();
            driver.Quit();

            TestSteps.Step = 0;
            Assert.Fail(exception.Message);
        }
    }
}