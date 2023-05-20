using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace TestExecutor;

public partial class MainPage : ContentPage
{
	public MainPage() => InitializeComponent();

	protected override void OnAppearing()
	{
		base.OnAppearing();

		Connectivity.ConnectivityChanged += ConnectivityCheck;
	}

	public async void ConnectivityCheck(Object sender, ConnectivityChangedEventArgs args)
	{
		if (args.NetworkAccess == NetworkAccess.Internet)
			await Toast.Make("Internet connection restored!", ToastDuration.Long, 14).Show(default);
		else
			await Toast.Make("Internet connection lost!", ToastDuration.Long, 14).Show(default);
	}
}