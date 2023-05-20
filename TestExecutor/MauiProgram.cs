using Microsoft.Maui.LifecycleEvents;
using Microsoft.Extensions.Logging;

using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Maui;

using TestExecutor.Services;
using TestExecutor.Models;

using System.Runtime.InteropServices;

namespace TestExecutor;

public static partial class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();

		builder.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();
		#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
		#endif

		builder.Services.AddSingleton<ITestApplicationsStore<TestApplication>, TestApplicationsDataStore>();
		builder.Services.AddSingleton<IBusinessProcessesStore<BusinessProcess>, BusinessProcessesDataStore>();
		builder.Services.AddSingleton<ISuiteExecutionsStore<SuiteExecution>, SuiteExecutionsDataStore>();
		builder.Services.AddSingleton<ITestCasesStore<TestCase>, TestCasesDataStore>();
		builder.Services.AddSingleton<IExecutionsStore<Execution>, ExecutionsDataStore>();
		builder.Services.AddSingleton<IDefectsStore<Defect>, DefectsDataStore>();
		builder.Services.AddSingleton<IReportsStore<Execution>, ReportsDataStore>();
		builder.Services.AddSingleton<ICompaniesStore<Company>, CompaniesDataStore>();
		builder.Services.AddSingleton<IRolesStore<Role>, RolesDataStore>();
		builder.Services.AddSingleton<IAccountsStore<LoginViewModel, GetProfileViewModel, UpdateProfileViewModel, ChangePasswordViewModel>, AccountsDataStore>();
		builder.Services.AddSingleton<IUsersStore<User, RegisterUserViewModel, UpdateUserViewModel>, UsersDataStore>();
		builder.Services.AddSingleton(FileSaver.Default);
		builder.Services.AddTransient<MainPage>();

		#if WINDOWS
		builder.ConfigureLifecycleEvents(events =>
		{
			events.AddWindows(wndLifeCycleBuilder =>
			{
				wndLifeCycleBuilder.OnWindowCreated(window =>
				{
					IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
					ShowWindow(hWnd, 3);
				});
			});
		});
		#endif

		return builder.Build();
	}

	#if WINDOWS
	[LibraryImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static partial Boolean ShowWindow(IntPtr hWnd, Int32 cmdShow);
	#endif
}