using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MauiLicApp.WinUI;

using Babel.Licensing;
using Platforms.Windows;
using System.Diagnostics;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : MauiWinUIApplication
{
	/// <summary>
	/// Initializes the singleton application object.  This is the first line of authored code
	/// executed, and as such is the logical equivalent of main() or WinMain().
	/// </summary>
	public App()
	{
		this.InitializeComponent();

        UnhandledException += (sender, e) =>
        {
            // Handle unhandled exceptions here
            var ex = e.Exception;
            ex?.ReportUnhandledException();
        };
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        // Register the SystemInformation service for licensing
        LicenseServices.Current.AddService(typeof(ISystemInformation), new SystemInformation());

        base.OnLaunched(args);
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}

