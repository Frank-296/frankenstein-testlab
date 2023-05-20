namespace TestLab.TestApplications.AppleStore.Locators;

public class HomeLocators
{
	public static readonly Dictionary<String, String> Locators = new()
	{
		{ "continueButton_Id", "ac-ls-continue" },
		{ "searchResult_Xpath", "//span[text()='{0}']/parent::span" },
		{ "optionSelect_Xpath", "//a[text()='{0}']" }
	};
}