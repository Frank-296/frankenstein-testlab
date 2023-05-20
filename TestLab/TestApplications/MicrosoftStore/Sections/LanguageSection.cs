#nullable disable
using TestLab.TestApplications.MicrosoftStore.Locators;

namespace TestLab.TestApplications.MicrosoftStore.Sections;

public class LanguageSection
{
    static Screenshot screenshot;

    public static void ChangeLanguageStep(IWebDriver driver, List<DataPool> dataPool, ExtentReports report, ExtentTest test)
    {
        TestSteps.SetStepNumber();

        try
        {
            var loadPage = new WebDriverWait(driver, TimeSpan.FromSeconds(20))
                .Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

            if (loadPage)
            {
                var languageSelectionElements = driver.FindElements(By.XPath(LearnLocators.Locators["languageSelection_Xpath"]));

                foreach (var element in languageSelectionElements)
                    if (element.Displayed)
                        if (element.Enabled)
                        {
                            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);

                            element.Click();

                            Thread.Sleep(2000);

                            var newLanguageSelection = new WebDriverWait(driver, TimeSpan.FromSeconds(5))
                                .Until(d => d.FindElement(By.XPath(String.Format(LearnLocators.Locators["newLanguageSelection_Xpath"], dataPool
                                    .FirstOrDefault(x => x.Parameter == "Language").Value))));

                            if (newLanguageSelection.Displayed)
                            {
                                Assert.That(newLanguageSelection.Displayed, Is.EqualTo(true));

                                newLanguageSelection.Click();

								Thread.Sleep(2000);

								screenshot = driver.TakeScreenshot();

								var status = Status.Pass;
                                var step = TestSteps.Step + " The language selection was successful.";
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
            var step = TestSteps.Step + " The language selection failed.\n" + exception.Message;
            var evidence = MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot.AsBase64EncodedString, step).Build();

            test.Log(status, step, evidence);

            report.Flush();
            driver.Quit();

            TestSteps.Step = 0;
            Assert.Fail(exception.Message);
        }
    }
}