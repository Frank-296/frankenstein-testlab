namespace TestLab.TestApplications.MicrosoftStore.Locators;

public class HomeLocators
{
    public static readonly Dictionary<String, String> Locators = new()
    {
        { "footerSection_Xpath", "//footer//div//li/a[text()='{0}']" },
        { "searchInput_Id", "cli_shellHeaderSearchInput" },
        { "searchButton_Id", "search" },
        { "searchResult_Xpath", "//span//b[text()='{0}']" },
    };
}