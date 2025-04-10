using ObjCRuntime;
using UIKit;
using Babel.Licensing;

namespace MauiLicApp;

using Platforms.iOS;

public class Program
{
	// This is the main entry point of the application.
	static void Main(string[] args)
	{
        // Register the SystemInformation service for licensing
        LicenseServices.Current.AddService(typeof(ISystemInformation), new SystemInformation());
		
		// if you want to use a different Application Delegate class from "AppDelegate"
		// you can specify it here.
		UIApplication.Main(args, null, typeof(AppDelegate));
	}
}
