namespace TestLab.TestApplications.MicrosoftStore.Locators;

public class CodeSamplesLocators
{
    public static readonly Dictionary<String, String> Locators = new()
    {
        { "searchCodeSampleInput_Id", "facet-search-input" },
        { "searchCodeSampleButton_Id", "facet-search-submit" },
        { "products_Xpath", "//h3[text()='Products']" },
        { "productsAreaCheck_Xpath", "//label//span[text()='{0}']" }
    };
}