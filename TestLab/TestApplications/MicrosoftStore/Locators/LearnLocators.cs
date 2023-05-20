namespace TestLab.TestApplications.MicrosoftStore.Locators;

public class LearnLocators
{
    public static readonly Dictionary<String, String> Locators = new()
    {
        { "languageSelection_Xpath", "//span[text()='English (United States)']" },
        { "newLanguageSelection_Xpath", "//a[text()='{0}']" },
        { "searchInput_Id", "directory-page-search-form-autocomplete-input" },
        { "searchButton_Id", "directory-search-submit" },
        { "contentArea_Xpath", "//h3[text()='Área de contenido']" },
        { "contentAreaRadio_Xpath", "//span//span[text()='{0}']" },
        { "searchResult_Xpath", "//a[text()='{0}']" }
    };
}