using Android.App;
using Android.Runtime;
using Babel.Licensing;

namespace MauiLicApp;

using Platforms.Android;

[Application]
public class MainApplication : MauiApplication
{
	public MainApplication(IntPtr handle, JniHandleOwnership ownership)
		: base(handle, ownership)
	{
        // Register the SystemInformation service for licensing
        LicenseServices.Current.AddService(typeof(ISystemInformation), new SystemInformation());
	}

	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
