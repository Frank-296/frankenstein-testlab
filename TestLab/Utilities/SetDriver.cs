#nullable disable
namespace TestLab.Utilities;

public class SetDriver
{
	public static IWebDriver SelectBrowser(BrowserDriver browser)
    {
        try
		{
			IWebDriver driver = browser switch
			{
				BrowserDriver.Chrome => new ChromeDriver(),
				BrowserDriver.Edge => new EdgeDriver(),
				BrowserDriver.Firefox => new FirefoxDriver(),
				BrowserDriver.Safari => new SafariDriver(),
				_ => new ChromeDriver(),
			};

			return driver;
		}
		catch (Exception)
		{
			return null;
		}
	}
}

public enum BrowserDriver : Int32
{
	[Display(Name = "Google Chrome")]
	Chrome = 1,
	[Display(Name = "Microsoft Edge")]
	Edge = 2,
	[Display(Name = "Mozila Firefox")]
	Firefox = 3,
	[Display(Name = "Apple Safari")]
	Safari = 4
}