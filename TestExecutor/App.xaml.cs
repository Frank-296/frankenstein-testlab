namespace TestExecutor;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new MainPage();
	}

	protected override Window CreateWindow(IActivationState activationState)
	{
		var window = base.CreateWindow(activationState);

		window.Created += (s, e) =>
		{
			window.Title = "Frankenstein Test Lab";
		};

		return window;
	}
}