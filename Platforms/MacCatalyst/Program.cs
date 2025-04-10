using UIKit;
using Babel.Licensing;

namespace MauiLicApp;

using Platforms.MacCatalyst;

public class Program
{
	// This is the main entry point of the application.
	static void Main(string[] args)
	{
		// Register the MacCatalyst-specific SystemInformation implementation
		LicenseServices.Current.AddService(typeof(ISystemInformation), new SystemInformation());
		
		// Register the unhandled exception handler for ObjCRuntime
		ObjCRuntime.Runtime.MarshalManagedException += (_, args) => 
        {            
            args.Exception?.ReportUnhandledException();
        };

		// if you want to use a different Application Delegate class from "AppDelegate"
		// you can specify it here.
		UIApplication.Main(args, null, typeof(AppDelegate));
	}
}
