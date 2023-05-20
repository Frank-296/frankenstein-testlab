namespace TestLab.TestApplications.MicrosoftStore.Locators;

public class DeveloperLocators
{
    public static readonly Dictionary<String, String> Locators = new()
    {
        { "button_Xpath", "//a[text()='{0}']" },
        { "navOption_Xpath", "//span[text()='{0}']" }
    };
}